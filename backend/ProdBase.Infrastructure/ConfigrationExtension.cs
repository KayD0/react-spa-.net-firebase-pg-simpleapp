using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Cors.Infrastructure;
using ProdBase.Infrastructure.Repository;

namespace ProdBase.Infrastructure
{
    public static class ConfigrationExtension
    {
        public static IServiceCollection AddApplicationConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure database
            var connectionString = configuration.GetConnectionString("DefaultConnection") ??
                $"Host={configuration["DB_HOST"] ?? "localhost"};" +
                $"Port={configuration["DB_PORT"] ?? "5432"};" +
                $"Database={configuration["DB_NAME"] ?? "youtubeapp"};" +
                $"Username={configuration["DB_USER"] ?? "postgres"};" +
                $"Password={configuration["DB_PASSWORD"] ?? "postgres"};";

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString));

            // Configure CORS
            var corsOrigin = configuration["CORS_ORIGIN"] ?? "http://localhost:3000";
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins(corsOrigin)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

            return services;
        }
    }
}
