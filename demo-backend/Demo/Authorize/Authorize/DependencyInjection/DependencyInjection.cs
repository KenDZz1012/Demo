using Authorize.Application.Contracts.Persistence;
using Authorize.Infrastructure.Repositories;
using Service.Lib.HttpRequest;
using Service.Lib.Keycloak;

namespace Authorize.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddProjectServices(this IServiceCollection services)
        {
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IKeycloakService, KeycloakService>();
            services.AddScoped<IHttpRequestService, HttpRequestService>();
            return services;
        }
    }
}
