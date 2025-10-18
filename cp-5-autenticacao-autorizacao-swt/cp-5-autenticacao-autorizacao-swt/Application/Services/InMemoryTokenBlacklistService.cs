using cp_5_autenticacao_autorizacao_swt.Application.Interfaces;

namespace cp_5_autenticacao_autorizacao_swt.Application.Services;

/// <summary>
/// Implementação em memória do serviço de blacklist de tokens JWT.
/// Armazena tokens invalidados em um HashSet para verificação rápida.
/// </summary>
/// <remarks>
/// Esta implementação usa armazenamento em memória para simplicidade.
/// Em produção, considere usar Redis ou banco de dados para persistência
/// e escalabilidade entre múltiplas instâncias da aplicação.
/// </remarks>
public class InMemoryTokenBlacklistService : ITokenBlacklistService
{
    private readonly HashSet<string> _blacklistedTokens;
    private readonly ILogger<InMemoryTokenBlacklistService> _logger;

    /// <summary>
    /// Inicializa uma nova instância do serviço de blacklist em memória.
    /// </summary>
    /// <param name="logger">Logger para registrar operações</param>
    public InMemoryTokenBlacklistService(ILogger<InMemoryTokenBlacklistService> logger)
    {
        _blacklistedTokens = new HashSet<string>();
        _logger = logger;
    }

    /// <summary>
    /// Adiciona um token à lista negra usando seu JTI.
    /// </summary>
    /// <param name="jti">Identificador único do token JWT</param>
    /// <returns>Task representando a operação assíncrona</returns>
    public async Task AddToBlacklistAsync(string jti)
    {
        if (string.IsNullOrEmpty(jti))
        {
            _logger.LogWarning("Tentativa de adicionar JTI vazio à blacklist");
            return;
        }

        lock (_blacklistedTokens)
        {
            _blacklistedTokens.Add(jti);
        }

        _logger.LogInformation("Token JTI {Jti} adicionado à blacklist", jti);
        await Task.CompletedTask;
    }

    /// <summary>
    /// Verifica se um token está na lista negra.
    /// </summary>
    /// <param name="jti">Identificador único do token JWT</param>
    /// <returns>True se o token está blacklisted, false caso contrário</returns>
    public async Task<bool> IsBlacklistedAsync(string jti)
    {
        if (string.IsNullOrEmpty(jti))
        {
            return false;
        }

        bool isBlacklisted;
        lock (_blacklistedTokens)
        {
            isBlacklisted = _blacklistedTokens.Contains(jti);
        }

        if (isBlacklisted)
        {
            _logger.LogWarning("Token JTI {Jti} está na blacklist", jti);
        }

        await Task.CompletedTask;
        return isBlacklisted;
    }

    /// <summary>
    /// Remove tokens expirados da lista negra.
    /// Em uma implementação em memória, esta operação não é necessária,
    /// mas mantemos para compatibilidade com a interface.
    /// </summary>
    /// <returns>Número de tokens removidos (sempre 0 nesta implementação)</returns>
    public async Task<int> CleanupExpiredTokensAsync()
    {
        // Em uma implementação em memória, não precisamos limpar tokens expirados
        // pois eles serão removidos automaticamente quando a aplicação reiniciar.
        // Em uma implementação com Redis ou banco de dados, aqui seria feita a limpeza.
        
        _logger.LogInformation("Limpeza de tokens expirados solicitada (implementação em memória)");
        await Task.CompletedTask;
        return 0;
    }
}
