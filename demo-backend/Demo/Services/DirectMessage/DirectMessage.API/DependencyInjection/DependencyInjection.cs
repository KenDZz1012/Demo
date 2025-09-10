using DirectMessage.Application.Contracts.Persistence;
using DirectMessage.Infrastructure.Repositories;
using Service.Lib.HttpRequest;
using Service.Lib.Minio;

namespace DirectMessage.API.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddProjectServices(this IServiceCollection services)
        {
            // Đăng ký service và repository
            services.AddScoped<IMinioService, MinioService>();
            services.AddScoped<IDirectMessageRepository, DirectMessageRepository>();
            services.AddScoped<IParticipantRepository, ParticipantRepository>();
            services.AddScoped<IConversationRepository, ConversationRepository>();
            services.AddScoped<IReadReceiptRepository, ReadReceiptRepository>();
            return services;
        }
    }
}
