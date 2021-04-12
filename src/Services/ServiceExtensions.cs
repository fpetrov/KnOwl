using KnOwl.Repositories;
using KnOwl.Repositories.Article;
using KnOwl.Repositories.Image;
using KnOwl.Repositories.User;
using KnOwl.Services.Article;
using KnOwl.Services.Image;
using KnOwl.Services.User;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace KnOwl.Services
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddUserService(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }

        public static IServiceCollection AddArticleService(this IServiceCollection services)
        {
            services.AddScoped<IArticleRepository, ArticleRepository>();
            services.AddScoped<IArticleService, ArticleService>();

            return services;
        }

        public static IServiceCollection AddImageService(this IServiceCollection services)
        {
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<IImageService, ImageService>();

            return services;
        }

        public static IServiceCollection AddSecurityService(this IServiceCollection services)
        {
            services.AddScoped<ISecurityService, SecurityService>();

            return services;
        }
    }

    public static class ControllerBaseExtensions
    {
        public static string GetUserName(this HttpContext context)
        {
            return GetClaimValue(context, "name");
        }

        private static string GetClaimValue(HttpContext context, string claim)
        {
            return context.User.Claims.FirstOrDefault(c => c.Type == claim).Value;
        }
    }
}