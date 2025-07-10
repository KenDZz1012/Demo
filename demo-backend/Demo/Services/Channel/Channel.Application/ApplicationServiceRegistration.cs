using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Channel.Application.Behaviours;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Service.Lib.Minio;
using FluentValidation;
using MediatR;

namespace Channel.Application
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
