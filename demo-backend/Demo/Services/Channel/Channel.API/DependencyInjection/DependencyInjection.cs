using Channel.Application.Contracts.Persistence;
using Channel.Infrastructure.Repositories;
using Service.Lib.Minio;

namespace Channel.API.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddProjectServices(this IServiceCollection services)
        {
            // Đăng ký service và repository
            services.AddScoped<IMinioService, MinioService>();
            services.AddScoped<IChannelRepository, ChannelRepository>();
            services.AddScoped<IServerRepository, ServerRepository>();
            services.AddScoped<IServerMemberRepository, ServerMemberRepository>();
            services.AddScoped<IServerInviteLinkRepository, ServerInviteLinkRepository>(); 
            return services;
        }
    }
}