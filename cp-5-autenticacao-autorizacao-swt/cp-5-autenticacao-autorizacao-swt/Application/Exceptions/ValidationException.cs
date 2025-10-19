namespace cp_5_autenticacao_autorizacao_swt.Application.Exceptions;

/// <summary>
/// Exceção para erros de validação de dados.
/// </summary>
/// <remarks>
/// Esta exceção é lançada quando dados de entrada não passam na validação,
/// contendo detalhes específicos sobre os campos que falharam.
/// </remarks>
public class ValidationException : BusinessException
{
    /// <summary>
    /// Dicionário com os erros de validação por campo.
    /// </summary>
    public Dictionary<string, string[]> ValidationErrors { get; }

    /// <summary>
    /// Inicializa uma nova instância da exceção de validação.
    /// </summary>
    /// <param name="validationErrors">Erros de validação por campo</param>
    /// <param name="message">Mensagem de erro (opcional)</param>
    public ValidationException(Dictionary<string, string[]> validationErrors, string? message = null) 
        : base(message ?? "Um ou mais erros de validação ocorreram.", "VALIDATION_ERROR", validationErrors)
    {
        ValidationErrors = validationErrors;
    }

    /// <summary>
    /// Inicializa uma nova instância da exceção de validação com erro simples.
    /// </summary>
    /// <param name="field">Campo com erro</param>
    /// <param name="error">Mensagem de erro</param>
    public ValidationException(string field, string error) 
        : this(new Dictionary<string, string[]> { { field, new[] { error } } })
    {
    }
}



