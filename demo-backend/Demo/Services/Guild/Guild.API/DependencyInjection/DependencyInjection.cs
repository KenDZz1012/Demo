using Guild.Application.Contracts.Persistence;
using Guild.Infrastructure.Repositories;
using Service.Lib.Minio;

namespace Guild.API.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddProjectServices(this IServiceCollection services)
        {
            services.AddScoped<IMinioService, MinioService>();
            services.AddScoped<IGuildRepository, GuildRepository>();
            services.AddScoped<IGuildMemberRepository, GuildMemberRepository>();
            services.AddScoped<IGuildInviteRepository, GuildInviteRepository>();
            return services;
        }
    }
}
