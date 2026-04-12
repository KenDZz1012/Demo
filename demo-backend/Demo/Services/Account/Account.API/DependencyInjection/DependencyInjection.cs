using Account.Application.Contracts.Persistence;
using Account.Infrastructure.Mail;
using Account.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Servivce.HttpHelper.HttpHelper;
using Servivce.HttpHelper.Services;
using Service.Lib.Minio;

namespace Account.API.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddProjectServices(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserRelationshipRepository, UserRelationshipRepository>();
        services.AddScoped<IMinioService, MinioService>();
        services.AddScoped<IEmailService, EmailService>();

        services.AddHttpClient("DefaultHttpClient");
        services.AddScoped<HttpHelper>();
        services.AddScoped<AuthorizeHttpService>();

        return services;
    }
}
