using TurmaMaisA.Persistence;

namespace TurmaMaisA.Middlewares.OrganizationInjection
{
    public class OrganizationInjectionMiddleware
    {
        private readonly RequestDelegate _next;

        public OrganizationInjectionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, AppDbContext dbContext)
        {
            var organizationIdClaim = httpContext.User.FindFirst("OrganizationId");

            if (organizationIdClaim != null && Guid.TryParse(organizationIdClaim.Value, out var organizationId))
            {
                dbContext.OrganizationId = organizationId;
            }

            await _next(httpContext);
        }
    }
}
