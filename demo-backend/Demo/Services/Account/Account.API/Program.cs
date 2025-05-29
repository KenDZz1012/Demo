using Microsoft.EntityFrameworkCore;
using Account.Infrastructure.Data;
using Account.Infrastructure.Repositories;
using Account.API.DependencyInjection;
using Account.Application.Mappings;
using Account.Application.Behaviours;
using MediatR;
using Account.Application.Features.User.Commands.CreateUserCommand;
using FluentValidation;     
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Account.Application;
using Service.Lib.Minio;
using Microsoft.OpenApi.Models;
using Service.Lib.Keycloak;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddProjectServices();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(Environment.GetEnvironmentVariable("SQL_CONNECTION")));
builder.Services.AddApplicationServices();
builder.Services.AddSingleton<MinioContext>();
builder.Services.AddSingleton<MinioService>();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<KeycloakService>();
builder.Services.AddHttpClient<IKeycloakService, KeycloakService>()
    .ConfigurePrimaryHttpMessageHandler(() =>
    {
        return new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
        };
    });
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Tạo endpoint cho Swagger JSON (mặc định: /swagger/{documentName}/swagger.json)
    app.UseSwaggerUI();
}
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
