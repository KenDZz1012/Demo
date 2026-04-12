using Microsoft.Extensions.DependencyInjection;
using Servivce.HttpHelper.HttpHelper;
using Servivce.HttpHelper.Services;
using Service.Lib.HttpRequest;

namespace Authorize.DependencyInjection;

public static class DependencyInjection
{
    /// <summary>
    /// HttpClient + AccountHttpService phục vụ ProfileService (gọi Account API theo biến môi trường ACCOUNT_URL).
    /// </summary>
    public static IServiceCollection AddProjectServices(this IServiceCollection services)
    {
        services.AddHttpClient("DefaultHttpClient");
        services.AddScoped<HttpHelper>();
        services.AddScoped<AccountHttpService>();

        services.AddScoped<IHttpRequestService, HttpRequestService>();
        return services;
    }
}
