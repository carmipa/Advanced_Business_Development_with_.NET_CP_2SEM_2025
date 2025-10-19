namespace cp_5_autenticacao_autorizacao_swt.Application.Exceptions;

/// <summary>
/// Exceção para erros de autorização.
/// </summary>
/// <remarks>
/// Esta exceção é lançada quando um usuário não tem permissão
/// para realizar uma operação específica.
/// </remarks>
public class UnauthorizedException : BusinessException
{
    /// <summary>
    /// Operação que foi negada.
    /// </summary>
    public string? Operation { get; }

    /// <summary>
    /// Recurso que estava sendo acessado.
    /// </summary>
    public string? Resource { get; }

    /// <summary>
    /// Inicializa uma nova instância da exceção de não autorizado.
    /// </summary>
    /// <param name="message">Mensagem de erro</param>
    /// <param name="operation">Operação negada</param>
    /// <param name="resource">Recurso acessado</param>
    public UnauthorizedException(string message, string? operation = null, string? resource = null) 
        : base(message, "UNAUTHORIZED", new { Operation = operation, Resource = resource })
    {
        Operation = operation;
        Resource = resource;
    }
}


