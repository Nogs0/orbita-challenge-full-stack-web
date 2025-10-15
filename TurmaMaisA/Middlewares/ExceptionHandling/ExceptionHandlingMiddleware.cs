using System.Net;
using System.Text.Json;
using TurmaMaisA.Middlewares.ExceptionHandling.Dtos;

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
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = _env.IsDevelopment()
                    ? new ExceptionHandlingDto()
                    {
                        StatusCode = httpContext.Response.StatusCode,
                        Message = "Internal server error. Details: " + ex.Message,
                        Details = ex.StackTrace
                    }
                    : new ExceptionHandlingDto()
                    {
                        StatusCode = httpContext.Response.StatusCode,
                        Message = "Internal server error."
                    };

                var jsonResponse = JsonSerializer.Serialize(response);

                await httpContext.Response.WriteAsync(jsonResponse);
            }
        }
    }
}
