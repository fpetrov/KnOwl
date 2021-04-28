using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using KnOwl.Database.Contexts;
using Microsoft.EntityFrameworkCore;
using KnOwl.Middlewares;
using KnOwl.Services;

namespace KnOwl
{
    public class Startup
    {
        private readonly string _clientRootPath;

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            _clientRootPath = Path.Combine(Directory.GetCurrentDirectory(), Configuration["ClientRootPath"]);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            // Configure custom services.
            services.AddUserService(options =>
            {
                options.Issuer = Configuration["JWT:Issuer"];
                options.Audience = Configuration["JWT:Audience"];
                options.ServerSecret = Configuration["JWT:ServerSecret"];
                options.RefreshTokenLifeTime = 15;
            });
            services.AddArticleService();
            services.AddSecurityService();
            services.AddImageService();

            // Configure database.
            services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("ApplicationDbContext"), providerOptions =>
                {
                    providerOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                });
            });

            services
                .AddCors()
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    var serverSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:ServerSecret"]));

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = serverSecret,
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidIssuer = Configuration["JWT:Issuer"],
                        ValidateAudience = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidAudience = Configuration["JWT:Audience"],
                        ValidateLifetime = true,
                    };
                });
            
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = _clientRootPath;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseSecureHeaders();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors(builder => {
                builder
                    .AllowCredentials()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = _clientRootPath;

                spa.Options.DefaultPageStaticFileOptions = new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(_clientRootPath)
                };
            });
        }
    }
}