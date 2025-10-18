using System.Net;
using System.Text.Json;

namespace cp_5_autenticacao_autorizacao_swt.Presentation.Middleware
{
    /// <summary>
    /// Middleware para tratamento global de exceções
    /// </summary>
    /// <remarks>
    /// Este middleware captura todas as exceções não tratadas que ocorrem durante o processamento
    /// de requisições HTTP e retorna respostas padronizadas em formato JSON.
    /// Ele ignora rotas do Swagger para não interferir na documentação da API.
    /// </remarks>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        /// <summary>
        /// Construtor do middleware de tratamento de exceções
        /// </summary>
        /// <param name="next">Delegate para o próximo middleware na pipeline</param>
        /// <param name="logger">Logger para registrar exceções</param>
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Processa a requisição HTTP e captura exceções não tratadas
        /// </summary>
        /// <param name="context">Contexto da requisição HTTP</param>
        /// <returns>Task representando a operação assíncrona</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            // Ignora rotas do Swagger
            if (context.Request.Path.StartsWithSegments("/swagger"))
            {
                await _next(context);
                return;
            }

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exceção não tratada capturada");
                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// Trata a exceção capturada e retorna uma resposta padronizada em JSON
        /// </summary>
        /// <param name="context">Contexto da requisição HTTP</param>
        /// <param name="exception">Exceção capturada</param>
        /// <returns>Task representando a operação assíncrona</returns>
        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            
            var response = new
            {
                message = "Erro interno do servidor",
                timestamp = DateTime.UtcNow,
                path = context.Request.Path
            };

            // Define o status code baseado no tipo de exceção
            if (exception is UnauthorizedAccessException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                response = new
                {
                    message = "Acesso não autorizado",
                    timestamp = DateTime.UtcNow,
                    path = context.Request.Path
                };
            }
            else if (exception is ArgumentException || exception is ArgumentNullException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                response = new
                {
                    message = "Dados inválidos",
                    timestamp = DateTime.UtcNow,
                    path = context.Request.Path
                };
            }
            else if (exception is KeyNotFoundException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                response = new
                {
                    message = "Recurso não encontrado",
                    timestamp = DateTime.UtcNow,
                    path = context.Request.Path
                };
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(jsonResponse);
        }
    }

    /// <summary>
    /// Extensões para registrar o middleware de tratamento de exceções
    /// </summary>
    /// <remarks>
    /// Esta classe fornece métodos de extensão para facilitar o registro
    /// do middleware de tratamento de exceções na pipeline de requisições.
    /// </remarks>
    public static class ExceptionHandlingMiddlewareExtensions
    {
        /// <summary>
        /// Registra o middleware de tratamento de exceções na pipeline de requisições
        /// </summary>
        /// <param name="builder">Builder da aplicação</param>
        /// <returns>Builder da aplicação para encadeamento</returns>
        public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
