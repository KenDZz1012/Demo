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


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProjectServices();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(Environment.GetEnvironmentVariable("SQL_CONNECTION")));
builder.Services.AddApplicationServices();
builder.Services.AddSingleton<MinioContext>();
builder.Services.AddSingleton<MinioService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/acc/swagger/v1/swagger.json", "Service A API");
        c.RoutePrefix = "account-swagger"; // Swagger UI sẽ hiển thị tại /acc/swagger/index.html qua Kong
    });

}

app.UseAuthorization();

app.MapControllers();

app.Run();
