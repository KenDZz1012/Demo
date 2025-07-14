using Microsoft.EntityFrameworkCore;
using Account.Infrastructure.Data;
using Account.API.DependencyInjection;
using Account.Application;
using Account.Application.Models.Emails;
using Service.Lib.Minio;
using Service.Lib.Keycloak;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddControllers();

services.AddDbContext<AccountContext>(options =>
    options.UseNpgsql(Environment.GetEnvironmentVariable("SQL_CONNECTION")));

services.AddApplicationServices();
services.AddProjectServices();

services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));


services.AddHttpClient<KeycloakService>();
services.AddHttpClient<IKeycloakService, KeycloakService>()
        .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
        });

services.AddEndpointsApiExplorer();
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Account API", Version = "v1" });
});

services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();
app.Run();
