using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace KnOwl.Middlewares
{
    public class SecureMiddleware
    {
        private readonly RequestDelegate _next;

        public SecureMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            httpContext.Response.Headers.Add("X-Frame-Options", "deny");
            httpContext.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            httpContext.Response.Headers.Add("X-Xss-Protection", "1");

            await _next(httpContext);
        }
    }
}
