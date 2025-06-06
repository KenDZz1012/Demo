using Service.Lib.HttpRequest;

namespace Channel.API.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddProjectServices(this IServiceCollection services)
        {
            // Đăng ký service và repository
            services.AddScoped<IHttpRequestService, HttpRequestService>();

            return services;
        }
    }
}
