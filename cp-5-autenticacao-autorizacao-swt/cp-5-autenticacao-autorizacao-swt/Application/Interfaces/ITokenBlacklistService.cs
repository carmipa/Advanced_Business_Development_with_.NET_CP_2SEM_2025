namespace cp_5_autenticacao_autorizacao_swt.Application.Interfaces;

/// <summary>
/// Interface para serviço de blacklist de tokens JWT.
/// Permite invalidar tokens antes da expiração para implementar logout seguro.
/// </summary>
/// <remarks>
/// Este serviço é fundamental para o sistema de logout avançado, permitindo
/// que tokens sejam invalidados imediatamente quando um usuário faz logout,
/// mesmo antes da expiração natural do token.
/// </remarks>
public interface ITokenBlacklistService
{
    /// <summary>
    /// Adiciona um token à lista negra usando seu JTI (JWT ID).
    /// </summary>
    /// <param name="jti">Identificador único do token JWT</param>
    /// <returns>Task representando a operação assíncrona</returns>
    /// <example>
    /// <code>
    /// await tokenBlacklistService.AddToBlacklistAsync("550e8400-e29b-41d4-a716-446655440000");
    /// </code>
    /// </example>
    Task AddToBlacklistAsync(string jti);

    /// <summary>
    /// Verifica se um token está na lista negra.
    /// </summary>
    /// <param name="jti">Identificador único do token JWT</param>
    /// <returns>True se o token está blacklisted, false caso contrário</returns>
    /// <example>
    /// <code>
    /// bool isBlacklisted = await tokenBlacklistService.IsBlacklistedAsync("550e8400-e29b-41d4-a716-446655440000");
    /// </code>
    /// </example>
    Task<bool> IsBlacklistedAsync(string jti);

    /// <summary>
    /// Remove tokens expirados da lista negra para otimizar memória.
    /// </summary>
    /// <returns>Número de tokens removidos</returns>
    /// <remarks>
    /// Esta operação deve ser chamada periodicamente para limpar tokens expirados
    /// e evitar acúmulo desnecessário na memória.
    /// </remarks>
    Task<int> CleanupExpiredTokensAsync();
}
