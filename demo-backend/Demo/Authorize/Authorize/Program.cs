using Account.Grpc.Protos;
using Authorize.Application;
using Authorize.Application.Contracts.Persistence;
using Authorize.GrpcServices;
using Authorize.Infrastructure.Data;
using Authorize.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Service.Lib.Context;
using Service.Lib.HttpRequest;
using Service.Lib.Keycloak;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// Add Controllers
services.AddControllers();

// Configure Database
services.AddDbContext<AuthorizeContext>(options =>
    options.UseSqlServer(Environment.GetEnvironmentVariable("SQL_CONNECTION")));

builder.Services.AddApplicationServices();


services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddHttpContextAccessor();
services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
services.AddSingleton<HttpRequestService>();
services.AddScoped<IKeycloakService, KeycloakService>();
services.AddSingleton<HttpRequestService>();
services.AddScoped<IHttpRequestService, HttpRequestService>();
services.AddHttpClient<KeycloakService>();
services.AddHttpClient<IKeycloakService, KeycloakService>()
    .ConfigurePrimaryHttpMessageHandler(() =>
    {
        return new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
        };
    });

services.AddGrpcClient<AccountProtoSerivce.AccountProtoSerivceClient>(o => o.Address = new Uri("http://localhost:8011"));
services.AddScoped<UserGrpcService>();


// Add CORS policy - Allow all
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Tạo endpoint cho Swagger JSON (mặc định: /swagger/{documentName}/swagger.json)
    app.UseSwaggerUI();
}
app.UseAuthorization();
app.MapControllers();
app.Run();
