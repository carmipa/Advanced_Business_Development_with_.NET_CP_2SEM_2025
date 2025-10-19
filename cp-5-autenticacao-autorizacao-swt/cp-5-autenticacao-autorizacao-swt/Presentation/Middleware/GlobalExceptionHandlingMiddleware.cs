using cp_5_autenticacao_autorizacao_swt.Application.Exceptions;
using System.Net;
using System.Text.Json;

namespace cp_5_autenticacao_autorizacao_swt.Presentation.Middleware;

/// <summary>
/// Middleware global para tratamento de exceções da aplicação.
/// </summary>
/// <remarks>
/// Este middleware captura todas as exceções não tratadas e retorna
/// respostas HTTP apropriadas com mensagens personalizadas.
/// </remarks>
public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    /// <summary>
    /// Inicializa uma nova instância do middleware de tratamento de exceções.
    /// </summary>
    /// <param name="next">Próximo middleware no pipeline</param>
    /// <param name="logger">Logger para registrar erros</param>
    public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Executa o middleware para capturar e tratar exceções.
    /// </summary>
    /// <param name="context">Contexto da requisição HTTP</param>
    /// <returns>Task representando a operação assíncrona</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    /// <summary>
    /// Trata a exceção e retorna uma resposta HTTP apropriada.
    /// </summary>
    /// <param name="context">Contexto da requisição HTTP</param>
    /// <param name="exception">Exceção capturada</param>
    /// <returns>Task representando a operação assíncrona</returns>
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        var errorResponse = new ErrorResponse();

        switch (exception)
        {
            case ValidationException validationEx:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse = new ErrorResponse
                {
                    StatusCode = response.StatusCode,
                    Message = validationEx.Message,
                    ErrorCode = validationEx.ErrorCode,
                    Details = validationEx.ValidationErrors,
                    Timestamp = DateTime.UtcNow
                };
                _logger.LogWarning("Erro de validação: {Message}", validationEx.Message);
                break;

            case NotFoundException notFoundEx:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                errorResponse = new ErrorResponse
                {
                    StatusCode = response.StatusCode,
                    Message = notFoundEx.Message,
                    ErrorCode = notFoundEx.ErrorCode,
                    Details = notFoundEx.Details,
                    Timestamp = DateTime.UtcNow
                };
                _logger.LogWarning("Recurso não encontrado: {Message}", notFoundEx.Message);
                break;

            case UnauthorizedException unauthorizedEx:
                response.StatusCode = (int)HttpStatusCode.Forbidden;
                errorResponse = new ErrorResponse
                {
                    StatusCode = response.StatusCode,
                    Message = unauthorizedEx.Message,
                    ErrorCode = unauthorizedEx.ErrorCode,
                    Details = unauthorizedEx.Details,
                    Timestamp = DateTime.UtcNow
                };
                _logger.LogWarning("Acesso não autorizado: {Message}", unauthorizedEx.Message);
                break;

            case BusinessException businessEx:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse = new ErrorResponse
                {
                    StatusCode = response.StatusCode,
                    Message = businessEx.Message,
                    ErrorCode = businessEx.ErrorCode,
                    Details = businessEx.Details,
                    Timestamp = DateTime.UtcNow
                };
                _logger.LogWarning("Erro de negócio: {Message}", businessEx.Message);
                break;

            case ArgumentException argEx:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse = new ErrorResponse
                {
                    StatusCode = response.StatusCode,
                    Message = "Parâmetro inválido fornecido.",
                    ErrorCode = "INVALID_ARGUMENT",
                    Details = new { ArgumentName = argEx.ParamName, Message = argEx.Message },
                    Timestamp = DateTime.UtcNow
                };
                _logger.LogWarning("Argumento inválido: {Message}", argEx.Message);
                break;

            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                errorResponse = new ErrorResponse
                {
                    StatusCode = response.StatusCode,
                    Message = "Ocorreu um erro interno do servidor. Tente novamente mais tarde.",
                    ErrorCode = "INTERNAL_SERVER_ERROR",
                    Details = new { Type = exception.GetType().Name },
                    Timestamp = DateTime.UtcNow
                };
                _logger.LogError(exception, "Erro interno não tratado: {Message}", exception.Message);
                break;
        }

        var jsonResponse = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await response.WriteAsync(jsonResponse);
    }
}

/// <summary>
/// Modelo para resposta de erro padronizada.
/// </summary>
public class ErrorResponse
{
    /// <summary>
    /// Código de status HTTP.
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    /// Mensagem de erro amigável.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Código de erro interno da aplicação.
    /// </summary>
    public string ErrorCode { get; set; } = string.Empty;

    /// <summary>
    /// Detalhes adicionais sobre o erro.
    /// </summary>
    public object? Details { get; set; }

    /// <summary>
    /// Timestamp do erro.
    /// </summary>
    public DateTime Timestamp { get; set; }
}

