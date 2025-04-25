using Account.Application.Interfaces;
using Account.Application.Services;
using Account.Domain.Interfaces;
using Account.Infrastructure.Repositories;

namespace Account.API.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddProjectServices(this IServiceCollection services)
        {
            // Đăng ký service và repository
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserRelationshipService, UserRelationshipService>();
            services.AddScoped<IUserRelationshipRepository, UserRelationshipRepository>();
            return services;
        }
    }
}
