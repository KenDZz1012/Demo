using Channel.Application.Contracts.Persistence;
using Channel.Infrastructure.Repositories;
using Service.Lib.Minio;

namespace Channel.API.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddProjectServices(this IServiceCollection services)
        {
            services.AddScoped<IMinioService, MinioService>();
            services.AddScoped<IChannelRepository, ChannelRepository>();
            return services;
        }
    }
}
