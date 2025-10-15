using TurmaMaisA.Middlewares.ExceptionHandling;
using TurmaMaisA.Middlewares.OrganizationInjection;

namespace TurmaMaisA.Middlewares
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder) { 
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }

        public static IApplicationBuilder UseOrganizationInjection(this IApplicationBuilder builder) {
            return builder.UseMiddleware<OrganizationInjectionMiddleware>();
        }
    }
}
