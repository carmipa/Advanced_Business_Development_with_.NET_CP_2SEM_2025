using cp_5_autenticacao_autorizacao_swt.Application.Interfaces;
using System.Security.Claims;

namespace cp_5_autenticacao_autorizacao_swt.Presentation.Middleware;

/// <summary>
/// Middleware personalizado para verificar se tokens JWT estão na blacklist.
/// Este middleware deve ser executado após a autenticação para interceptar
/// requisições com tokens invalidos e retornar 401 Unauthorized.
/// </summary>
/// <remarks>
/// Este middleware implementa o sistema de logout avançado, permitindo
/// invalidar tokens antes da expiração. Ele verifica o JTI (JWT ID) de cada
/// token autenticado contra a blacklist e bloqueia o acesso se necessário.
/// </remarks>
public class JwtBlacklistMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<JwtBlacklistMiddleware> _logger;

    /// <summary>
    /// Inicializa uma nova instância do middleware de blacklist JWT.
    /// </summary>
    /// <param name="next">Próximo middleware no pipeline</param>
    /// <param name="logger">Logger para registrar operações</param>
    public JwtBlacklistMiddleware(RequestDelegate next, ILogger<JwtBlacklistMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Executa o middleware para verificar tokens na blacklist.
    /// </summary>
    /// <param name="context">Contexto da requisição HTTP</param>
    /// <param name="blacklistService">Serviço de blacklist injetado</param>
    /// <returns>Task representando a operação assíncrona</returns>
    public async Task InvokeAsync(HttpContext context, ITokenBlacklistService blacklistService)
    {
        // Verifica se o usuário está autenticado
        if (context.User?.Identity?.IsAuthenticated == true)
        {
            // Extrai o JTI (JWT ID) das claims
            var jtiClaim = context.User.FindFirst("jti");
            
            if (jtiClaim != null && !string.IsNullOrEmpty(jtiClaim.Value))
            {
                // Verifica se o token está na blacklist
                var isBlacklisted = await blacklistService.IsBlacklistedAsync(jtiClaim.Value);
                
                if (isBlacklisted)
                {
                    _logger.LogWarning("Acesso negado: Token JTI {Jti} está na blacklist", jtiClaim.Value);
                    
                    // Retorna 401 Unauthorized se o token estiver blacklisted
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Token inválido ou expirado");
                    return;
                }
                
                _logger.LogDebug("Token JTI {Jti} validado com sucesso", jtiClaim.Value);
            }
            else
            {
                _logger.LogWarning("Token autenticado sem JTI claim");
            }
        }

        // Continua para o próximo middleware se o token não estiver blacklisted
        await _next(context);
    }
}
