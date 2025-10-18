namespace cp_5_autenticacao_autorizacao_swt.Application.Exceptions;

/// <summary>
/// Exceção base para erros de negócio da aplicação.
/// </summary>
/// <remarks>
/// Esta classe serve como base para todas as exceções de negócio,
/// permitindo tratamento centralizado e mensagens personalizadas.
/// </remarks>
public class BusinessException : Exception
{
    /// <summary>
    /// Código de erro personalizado para identificação.
    /// </summary>
    public string ErrorCode { get; }

    /// <summary>
    /// Detalhes adicionais sobre o erro.
    /// </summary>
    public object? Details { get; }

    /// <summary>
    /// Inicializa uma nova instância da exceção de negócio.
    /// </summary>
    /// <param name="message">Mensagem de erro</param>
    /// <param name="errorCode">Código de erro</param>
    /// <param name="details">Detalhes adicionais</param>
    public BusinessException(string message, string errorCode = "BUSINESS_ERROR", object? details = null) 
        : base(message)
    {
        ErrorCode = errorCode;
        Details = details;
    }

    /// <summary>
    /// Inicializa uma nova instância da exceção de negócio com exceção interna.
    /// </summary>
    /// <param name="message">Mensagem de erro</param>
    /// <param name="innerException">Exceção interna</param>
    /// <param name="errorCode">Código de erro</param>
    /// <param name="details">Detalhes adicionais</param>
    public BusinessException(string message, Exception innerException, string errorCode = "BUSINESS_ERROR", object? details = null) 
        : base(message, innerException)
    {
        ErrorCode = errorCode;
        Details = details;
    }
}
