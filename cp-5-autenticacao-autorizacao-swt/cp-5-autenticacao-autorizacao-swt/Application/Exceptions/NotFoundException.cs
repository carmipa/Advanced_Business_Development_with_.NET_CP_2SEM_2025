namespace cp_5_autenticacao_autorizacao_swt.Application.Exceptions;

/// <summary>
/// Exceção para recursos não encontrados.
/// </summary>
/// <remarks>
/// Esta exceção é lançada quando um recurso específico não é encontrado
/// no sistema, como usuário, nota, etc.
/// </remarks>
public class NotFoundException : BusinessException
{
    /// <summary>
    /// Nome do recurso que não foi encontrado.
    /// </summary>
    public string ResourceName { get; }

    /// <summary>
    /// Identificador do recurso não encontrado.
    /// </summary>
    public object? ResourceId { get; }

    /// <summary>
    /// Inicializa uma nova instância da exceção de recurso não encontrado.
    /// </summary>
    /// <param name="resourceName">Nome do recurso</param>
    /// <param name="resourceId">ID do recurso</param>
    /// <param name="message">Mensagem personalizada (opcional)</param>
    public NotFoundException(string resourceName, object? resourceId = null, string? message = null) 
        : base(
            message ?? $"{resourceName} não encontrado{(resourceId != null ? $" com ID '{resourceId}'" : "")}.",
            "NOT_FOUND",
            new { ResourceName = resourceName, ResourceId = resourceId })
    {
        ResourceName = resourceName;
        ResourceId = resourceId;
    }
}

