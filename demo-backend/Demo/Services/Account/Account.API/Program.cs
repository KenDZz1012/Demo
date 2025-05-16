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
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Account API", Version = "v1" });

    // 👇 Thêm server URL để ghi đè base path thành /acc
    c.AddServer(new OpenApiServer { Url = "http://kendz.site:8000/acc" });

    // Hoặc sử dụng basePath cho OpenAPI 2.0
    // c.DocumentFilter<BasePathFilter>("/acc"); 
});
builder.Services.AddHttpClient<KeycloakService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Tạo endpoint cho Swagger JSON (mặc định: /swagger/{documentName}/swagger.json)
    app.UseSwaggerUI(c =>
    {
        // Đặt tên endpoint Swagger JSON và API
        c.SwaggerEndpoint("/account-swagger/swagger/v1/swagger.json", "Account API V1");

        // Thay đổi RoutePrefix để Swagger UI chạy tại /account-swagger/swagger/index.html
        c.RoutePrefix = "account-swagger/swagger";

        // (Tùy chọn) Customize giao diện Swagger UI
        c.DocumentTitle = "Account API Documentation";
    });
}

app.UseAuthorization();

app.MapControllers();

app.Run();
