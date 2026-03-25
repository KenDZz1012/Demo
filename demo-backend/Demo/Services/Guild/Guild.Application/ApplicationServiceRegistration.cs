using System.Reflection;
using FluentValidation;
using Guild.Application.Behaviours;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Service.Lib.Minio;

namespace Guild.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddSingleton<MinioContext>();
            services.AddSingleton<MinioService>();
            return services;
        }
    }
}
