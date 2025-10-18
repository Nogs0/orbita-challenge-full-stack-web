using System.Net;
using System.Text.Json;
using TurmaMaisA.Utils.Exceptions;

namespace TurmaMaisA.Middlewares.ExceptionHandling
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger,
            IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);

                httpContext.Response.ContentType = "application/json";
                object responseBody = new { message = "Ocorreu um erro interno no servidor." };

                switch (ex)
                {
                    case NotFoundException notFoundException:
                        httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                        responseBody = new { message = "Recurso não encontrado." };
                        break;

                    case BusinessRuleException businessRuleException:
                        httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        responseBody = new { message = businessRuleException.Message };
                        break;

                    default:
                        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                                                                                   
                        if (_env.IsDevelopment())
                        {
                            responseBody = new { message = ex.Message, details = ex.StackTrace };
                        }
                        break;
                }

                var jsonResponse = JsonSerializer.Serialize(responseBody);
                await httpContext.Response.WriteAsync(jsonResponse);
            }
        }
    }
}
