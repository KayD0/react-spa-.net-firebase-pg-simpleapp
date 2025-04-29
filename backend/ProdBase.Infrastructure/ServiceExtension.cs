using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.SwaggerGen;
using ProdBase.Application.Interfaces;
using ProdBase.Application.Services;
using ProdBase.Domain.Interfaces;
using ProdBase.Infrastructure.Auth;
using ProdBase.Infrastructure.Repository;

namespace ProdBase.Infrastructure
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Register repositories
            services.AddScoped<IUserProfileRepository, UserProfileRepository>();

            // Register services
            services.AddSingleton<IAuthService, FirebaseAuth>();
            services.AddScoped<IProfileService, ProfileService>();

            // Add API Explorer and Swagger
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            return services;
        }
    }
}
