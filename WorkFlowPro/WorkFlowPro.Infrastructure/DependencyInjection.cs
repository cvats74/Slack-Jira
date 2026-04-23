using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WorkFlowPro.Application.Common.Interfaces;
using WorkFlowPro.Infrastructure.Data;
using WorkFlowPro.Infrastructure.Repositories;
using WorkFlowPro.Infrastructure.Services;

namespace WorkFlowPro.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Register DbContext ONCE here only
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString(
                        "DefaultConnection"),
                    b => b.EnableRetryOnFailure(3)));

            // Repositories
            services.AddScoped<IUserRepository,
                UserRepository>();
            services.AddScoped<IOrganizationRepository,
                OrganizationRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();



            // Services
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<IProjectService, ProjectService>();

            return services;
        }
    }
}