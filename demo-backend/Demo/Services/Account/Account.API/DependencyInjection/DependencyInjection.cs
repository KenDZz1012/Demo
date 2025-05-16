using Account.Application.Contracts.Persistence;
using Account.Infrastructure.Repositories;
using Service.Lib.Keycloak;
using Service.Lib.Minio;

namespace Account.API.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddProjectServices(this IServiceCollection services)
        {
            // Đăng ký service và repository
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserRelationshipRepository, UserRelationshipRepository>();
            services.AddScoped<IMinioService, MinioService>();
            services.AddScoped<IKeycloakService, KeycloakService>();
            return services;
        }
    }
}
