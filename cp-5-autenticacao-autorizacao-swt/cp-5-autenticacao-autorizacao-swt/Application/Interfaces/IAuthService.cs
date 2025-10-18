using cp_5_autenticacao_autorizacao_swt.Application.DTOs.Auth;

namespace cp_5_autenticacao_autorizacao_swt.Application.Interfaces
{
    /// <summary>
    /// Interface que define as operações de autenticação e autorização
    /// </summary>
    /// <remarks>
    /// Esta interface define o contrato para todas as operações relacionadas à autenticação
    /// e autorização de usuários no sistema. Inclui métodos para login, registro, renovação
    /// de tokens, logout e validação de tokens JWT.
    /// </remarks>
    public interface IAuthService
    {
        /// <summary>
        /// Realiza o login do usuário com email e senha
        /// </summary>
        /// <param name="loginRequest">Dados de login contendo email e senha</param>
        /// <returns>Resposta com token JWT, refresh token e informações do usuário, ou null se credenciais inválidas</returns>
        Task<LoginResponseDto?> LoginAsync(LoginRequestDto loginRequest);
        
        /// <summary>
        /// Registra um novo usuário no sistema
        /// </summary>
        /// <param name="registerRequest">Dados do novo usuário incluindo nome, email e senha</param>
        /// <returns>Resposta com token JWT, refresh token e informações do usuário criado, ou null se email já existe</returns>
        Task<LoginResponseDto?> RegisterAsync(RegisterRequestDto registerRequest);
        
        /// <summary>
        /// Renova o token JWT usando o refresh token
        /// </summary>
        /// <param name="refreshTokenRequest">Objeto contendo o refresh token válido</param>
        /// <returns>Novo token JWT, novo refresh token e informações do usuário, ou null se refresh token inválido</returns>
        Task<LoginResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto refreshTokenRequest);
        
        /// <summary>
        /// Realiza logout do usuário invalidando o refresh token
        /// </summary>
        /// <param name="userId">ID do usuário que está fazendo logout</param>
        /// <returns>True se logout realizado com sucesso, false caso contrário</returns>
        Task<bool> LogoutAsync(int userId);
        
        /// <summary>
        /// Valida se um token JWT é válido e não expirado
        /// </summary>
        /// <param name="token">Token JWT a ser validado</param>
        /// <returns>True se token é válido e não expirado, false caso contrário</returns>
        Task<bool> ValidateTokenAsync(string token);
        
        /// <summary>
        /// Obtém o ID do usuário a partir de um token JWT válido
        /// </summary>
        /// <param name="token">Token JWT válido</param>
        /// <returns>ID do usuário ou null se token inválido ou expirado</returns>
        Task<int?> GetUserIdFromTokenAsync(string token);
    }
}
