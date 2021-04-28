using Microsoft.AspNetCore.Builder;

namespace KnOwl.Middlewares
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseSecureHeaders(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<SecureMiddleware>();

            return builder;
        }
    }
}
