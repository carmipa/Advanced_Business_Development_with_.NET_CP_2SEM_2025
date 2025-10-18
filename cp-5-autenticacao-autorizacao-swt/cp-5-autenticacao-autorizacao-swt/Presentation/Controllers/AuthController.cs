using cp_5_autenticacao_autorizacao_swt.Application.DTOs.Auth;
using cp_5_autenticacao_autorizacao_swt.Application.Interfaces;
using cp_5_autenticacao_autorizacao_swt.Application.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace cp_5_autenticacao_autorizacao_swt.Presentation.Controllers
{
    /// <summary>
    /// Controller responsável por operações de autenticação
    /// Endpoints para login, registro, renovação de token e logout
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IValidator<LoginRequestDto> _loginValidator;
        private readonly IValidator<RegisterRequestDto> _registerValidator;
        private readonly ITokenBlacklistService _tokenBlacklistService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IAuthService authService,
            IValidator<LoginRequestDto> loginValidator,
            IValidator<RegisterRequestDto> registerValidator,
            ITokenBlacklistService tokenBlacklistService,
            ILogger<AuthController> logger)
        {
            _authService = authService;
            _loginValidator = loginValidator;
            _registerValidator = registerValidator;
            _tokenBlacklistService = tokenBlacklistService;
            _logger = logger;
        }

        /// <summary>
        /// Realiza login do usuário no sistema
        /// </summary>
        /// <remarks>
        /// Este endpoint permite que um usuário faça login fornecendo email e senha.
        /// Após a validação das credenciais, retorna um token JWT e refresh token para autenticação.
        /// 
        /// Exemplo de requisição:
        /// ```
        /// POST /api/auth/login
        /// {
        ///   "email": "usuario@exemplo.com",
        ///   "senha": "MinhaSenh@123"
        /// }
        /// ```
        /// </remarks>
        /// <param name="loginRequest">Dados de login contendo email e senha</param>
        /// <returns>Token JWT, refresh token e informações do usuário autenticado</returns>
        /// <response code="200">Login realizado com sucesso. Retorna token JWT e dados do usuário</response>
        /// <response code="400">Dados de entrada inválidos ou formato incorreto</response>
        /// <response code="401">Credenciais inválidas ou usuário não encontrado</response>
        /// <response code="500">Erro interno do servidor durante o processo de login</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponseDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
        {
            try
            {
                // Valida os dados de entrada
                var validationResult = await _loginValidator.ValidateAsync(loginRequest);
                if (!validationResult.IsValid)
                {
                    _logger.LogWarning("Tentativa de login com dados inválidos: {Errors}", 
                        string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
                    return BadRequest(new { errors = validationResult.Errors.Select(e => e.ErrorMessage) });
                }

                // Realiza o login
                var result = await _authService.LoginAsync(loginRequest);
                if (result == null)
                {
                    _logger.LogWarning("Tentativa de login falhada para email: {Email}", loginRequest.Email);
                    return Unauthorized(new { message = "Credenciais inválidas" });
                }

                _logger.LogInformation("Login realizado com sucesso para email: {Email}", loginRequest.Email);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao realizar login para email: {Email}", loginRequest.Email);
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Registra um novo usuário no sistema
        /// </summary>
        /// <remarks>
        /// Este endpoint permite criar uma nova conta de usuário no sistema.
        /// O usuário será criado com status ativo e role padrão "User".
        /// Após o registro bem-sucedido, retorna automaticamente um token JWT para login.
        /// 
        /// Exemplo de requisição:
        /// ```
        /// POST /api/auth/register
        /// {
        ///   "nome": "João Silva",
        ///   "email": "joao@exemplo.com",
        ///   "senha": "MinhaSenh@123",
        ///   "confirmarSenha": "MinhaSenh@123"
        /// }
        /// ```
        /// </remarks>
        /// <param name="registerRequest">Dados do novo usuário incluindo nome, email e senha</param>
        /// <returns>Token JWT, refresh token e informações do usuário recém-criado</returns>
        /// <response code="201">Usuário criado com sucesso. Retorna token JWT e dados do usuário</response>
        /// <response code="400">Dados de entrada inválidos ou senhas não coincidem</response>
        /// <response code="409">Email já está em uso por outro usuário</response>
        /// <response code="500">Erro interno do servidor durante o processo de registro</response>
        [HttpPost("register")]
        [ProducesResponseType(typeof(LoginResponseDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequest)
        {
            try
            {
                // Valida os dados de entrada
                var validationResult = await _registerValidator.ValidateAsync(registerRequest);
                if (!validationResult.IsValid)
                {
                    _logger.LogWarning("Tentativa de registro com dados inválidos: {Errors}", 
                        string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
                    return BadRequest(new { errors = validationResult.Errors.Select(e => e.ErrorMessage) });
                }

                // Realiza o registro
                var result = await _authService.RegisterAsync(registerRequest);
                if (result == null)
                {
                    _logger.LogWarning("Tentativa de registro falhada - email já em uso: {Email}", registerRequest.Email);
                    return Conflict(new { message = "Email já está em uso" });
                }

                _logger.LogInformation("Usuário registrado com sucesso: {Email}", registerRequest.Email);
                return CreatedAtAction(nameof(Login), new { email = registerRequest.Email }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao registrar usuário: {Email}", registerRequest.Email);
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }

        /// <summary>
        /// Renova o token JWT usando refresh token
        /// </summary>
        /// <remarks>
        /// Este endpoint permite renovar um token JWT expirado usando um refresh token válido.
        /// O refresh token deve estar ativo e não expirado para que a renovação seja bem-sucedida.
        /// Após a renovação, um novo token JWT e refresh token são gerados.
        /// 
        /// Exemplo de requisição:
        /// ```
        /// POST /api/auth/refresh-token
        /// {
        ///   "refreshToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
        /// }
        /// ```
        /// </remarks>
        /// <param name="refreshTokenRequest">Objeto contendo o refresh token válido</param>
        /// <returns>Novo token JWT, novo refresh token e informações do usuário</returns>
        /// <response code="200">Token renovado com sucesso. Retorna novos tokens</response>
        /// <response code="400">Refresh token inválido, expirado ou não encontrado</response>
        /// <response code="500">Erro interno do servidor durante a renovação</response>
        [HttpPost("refresh-token")]
        [ProducesResponseType(typeof(LoginResponseDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto refreshTokenRequest)
        {
            try
            {
                var result = await _authService.RefreshTokenAsync(refreshTokenRequest);
                if (result == null)
                {
                    _logger.LogWarning("Tentativa de renovação de token falhada");
                    return BadRequest(new { message = "Refresh token inválido ou expirado" });
                }

                _logger.LogInformation("Token renovado com sucesso");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao renovar token");
                return StatusCode(500, new { message = "Erro interno do servidor" });
            }
        }


        /// <summary>
        /// Valida se o token JWT é válido e retorna informações do usuário
        /// </summary>
        /// <remarks>
        /// Este endpoint permite verificar se um token JWT é válido e obter informações do usuário autenticado.
        /// Útil para verificar a validade do token antes de realizar operações sensíveis.
        /// Requer autenticação via token JWT válido.
        /// 
        /// Exemplo de requisição:
        /// ```
        /// GET /api/auth/validate
        /// Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
        /// ```
        /// </remarks>
        /// <returns>Informações do usuário autenticado e status de validação</returns>
        /// <response code="200">Token válido. Retorna informações do usuário</response>
        /// <response code="401">Token JWT inválido, expirado ou malformado</response>
        [HttpGet("validate")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public IActionResult Validate()
        {
            var userId = User.FindFirst("userId")?.Value;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            return Ok(new
            {
                message = "Token válido",
                userId = userId,
                email = email,
                role = role
            });
        }

        /// <summary>
        /// Realiza logout do usuário invalidando o token atual
        /// </summary>
        /// <remarks>
        /// Este endpoint implementa logout avançado com blacklist de tokens.
        /// O token JWT atual é adicionado à lista negra, impedindo seu reuso.
        /// 
        /// **Funcionalidade Avançada**: Este endpoint resolve o problema de tokens JWT
        /// serem "stateless" por natureza, permitindo invalidar tokens antes da expiração.
        /// 
        /// Exemplo de uso:
        /// ```
        /// POST /api/auth/logout
        /// Authorization: Bearer {token}
        /// ```
        /// </remarks>
        /// <returns>Mensagem de confirmação do logout</returns>
        /// <response code="200">Logout realizado com sucesso</response>
        /// <response code="401">Token inválido ou não fornecido</response>
        [HttpPost("logout")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Logout()
        {
            try
            {
                // Extrai o JTI (JWT ID) do token atual
                var jtiClaim = User.FindFirst("jti");
                
                if (jtiClaim == null || string.IsNullOrEmpty(jtiClaim.Value))
                {
                    _logger.LogWarning("Tentativa de logout com token sem JTI");
                    return BadRequest("Token inválido");
                }

                // Adiciona o token à blacklist
                await _tokenBlacklistService.AddToBlacklistAsync(jtiClaim.Value);

                _logger.LogInformation("Logout realizado com sucesso para token JTI: {Jti}", jtiClaim.Value);

                return Ok(new
                {
                    message = "Logout realizado com sucesso. Token invalidado.",
                    timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro durante logout");
                return StatusCode(500, "Erro interno do servidor");
            }
        }
    }
}
