using AutoMapper;
using cp_5_autenticacao_autorizacao_swt.Application.DTOs.Auth;
using cp_5_autenticacao_autorizacao_swt.Application.Interfaces;
using cp_5_autenticacao_autorizacao_swt.Domain.Entities;
using cp_5_autenticacao_autorizacao_swt.Domain.Enums;
using cp_5_autenticacao_autorizacao_swt.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BCrypt.Net;

namespace cp_5_autenticacao_autorizacao_swt.Application.Services
{
    /// <summary>
    /// Serviço responsável por operações de autenticação e autorização
    /// </summary>
    /// <remarks>
    /// Este serviço implementa todas as operações relacionadas à autenticação de usuários,
    /// incluindo login, registro, renovação de tokens JWT, logout e validação de tokens.
    /// Utiliza BCrypt para hash de senhas e JWT para autenticação stateless.
    /// </remarks>
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        /// <summary>
        /// Construtor do serviço de autenticação
        /// </summary>
        /// <param name="userRepository">Repositório para operações com usuários</param>
        /// <param name="configuration">Configurações da aplicação</param>
        /// <param name="mapper">Mapper para conversão entre DTOs e entidades</param>
        public AuthService(
            IUserRepository userRepository,
            IConfiguration configuration,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _mapper = mapper;
        }

        /// <summary>
        /// Realiza o login do usuário com email e senha
        /// </summary>
        /// <param name="loginRequest">Dados de login contendo email e senha</param>
        /// <returns>Resposta com tokens e dados do usuário, ou null se credenciais inválidas</returns>
        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto loginRequest)
        {
            // Busca o usuário pelo email
            var user = await _userRepository.GetByEmailAsync(loginRequest.Email);
            if (user == null)
                return null;

            // Verifica se o usuário está ativo
            if (user.Status != UserStatus.Active)
                return null;

            // Verifica se o usuário está bloqueado
            if (user.Status == UserStatus.Blocked)
                return null;

            // Verifica a senha
            if (!BCrypt.Net.BCrypt.Verify(loginRequest.Senha, user.SenhaHash))
            {
                // Incrementa tentativas de login falhadas
                user.TentativasLoginFalhadas++;
                if (user.TentativasLoginFalhadas >= 5)
                {
                    user.Status = UserStatus.Blocked;
                    user.DataBloqueio = DateTime.UtcNow;
                }
                await _userRepository.UpdateAsync(user);
                return null;
            }

            // Reset das tentativas de login falhadas
            user.TentativasLoginFalhadas = 0;
            user.UltimoLogin = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);

            // Gera os tokens
            var token = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken();

            // Atualiza o refresh token no usuário
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7); // 7 dias
            await _userRepository.UpdateAsync(user);

            return new LoginResponseDto
            {
                Token = token,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(60), // 1 hora
                User = _mapper.Map<UserDto>(user)
            };
        }

        /// <summary>
        /// Registra um novo usuário no sistema
        /// </summary>
        /// <param name="registerRequest">Dados do novo usuário incluindo nome, email e senha</param>
        /// <returns>Resposta com tokens e dados do usuário criado, ou null se email já existe</returns>
        public async Task<LoginResponseDto?> RegisterAsync(RegisterRequestDto registerRequest)
        {
            // Verifica se o email já existe
            if (await _userRepository.EmailExistsAsync(registerRequest.Email))
                return null;

            // Cria o novo usuário
            var user = new User
            {
                Nome = registerRequest.Nome,
                Email = registerRequest.Email,
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(registerRequest.Senha),
                Role = UserRole.User,
                Status = UserStatus.Active
            };

            // Salva o usuário
            var createdUser = await _userRepository.AddAsync(user);

            // Gera os tokens
            var token = GenerateJwtToken(createdUser);
            var refreshToken = GenerateRefreshToken();

            // Atualiza o refresh token no usuário
            createdUser.RefreshToken = refreshToken;
            createdUser.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userRepository.UpdateAsync(createdUser);

            return new LoginResponseDto
            {
                Token = token,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(60),
                User = _mapper.Map<UserDto>(createdUser)
            };
        }

        /// <summary>
        /// Renova o token JWT usando o refresh token
        /// </summary>
        /// <param name="refreshTokenRequest">Objeto contendo o refresh token válido</param>
        /// <returns>Novos tokens e dados do usuário, ou null se refresh token inválido</returns>
        public async Task<LoginResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto refreshTokenRequest)
        {
            var user = await _userRepository.GetByEmailAsync(GetEmailFromToken(refreshTokenRequest.RefreshToken));
            
            if (user == null || user.RefreshToken != refreshTokenRequest.RefreshToken || 
                user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                return null;

            // Gera novos tokens
            var token = GenerateJwtToken(user);
            var newRefreshToken = GenerateRefreshToken();

            // Atualiza o refresh token
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userRepository.UpdateAsync(user);

            return new LoginResponseDto
            {
                Token = token,
                RefreshToken = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(60),
                User = _mapper.Map<UserDto>(user)
            };
        }

        /// <summary>
        /// Realiza logout do usuário invalidando o refresh token
        /// </summary>
        /// <param name="userId">ID do usuário que está fazendo logout</param>
        /// <returns>True se logout realizado com sucesso, false se usuário não encontrado</returns>
        public async Task<bool> LogoutAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return false;

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;
            await _userRepository.UpdateAsync(user);

            return true;
        }

        /// <summary>
        /// Valida se um token JWT é válido e não expirado
        /// </summary>
        /// <param name="token">Token JWT a ser validado</param>
        /// <returns>True se token é válido, false caso contrário</returns>
        public Task<bool> ValidateTokenAsync(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]!);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Audience"],
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return Task.FromResult(true);
            }
            catch
            {
                return Task.FromResult(false);
            }
        }

        /// <summary>
        /// Obtém o ID do usuário a partir de um token JWT válido
        /// </summary>
        /// <param name="token">Token JWT válido</param>
        /// <returns>ID do usuário ou null se token inválido</returns>
        public Task<int?> GetUserIdFromTokenAsync(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jsonToken = tokenHandler.ReadJwtToken(token);
                var userIdClaim = jsonToken.Claims.FirstOrDefault(x => x.Type == "userId");
                
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                    return Task.FromResult<int?>(userId);
                
                return Task.FromResult<int?>(null);
            }
            catch
            {
                return Task.FromResult<int?>(null);
            }
        }

        /// <summary>
        /// Gera um token JWT para o usuário especificado
        /// </summary>
        /// <param name="user">Usuário para o qual o token será gerado</param>
        /// <returns>Token JWT assinado</returns>
        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]!);
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("userId", user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Nome),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(60),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Gera um refresh token aleatório seguro
        /// </summary>
        /// <returns>Refresh token em formato Base64</returns>
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        /// <summary>
        /// Extrai o email do usuário a partir de um token JWT
        /// </summary>
        /// <param name="token">Token JWT válido</param>
        /// <returns>Email do usuário ou string vazia se não encontrado</returns>
        private string GetEmailFromToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jsonToken = tokenHandler.ReadJwtToken(token);
                return jsonToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value ?? string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
