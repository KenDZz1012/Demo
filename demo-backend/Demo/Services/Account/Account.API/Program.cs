using Microsoft.EntityFrameworkCore;
using Account.Infrastructure.Data;
using Account.API.DependencyInjection;
using Account.Application;
using Account.Application.Models.Emails;
using Service.Lib.Minio;
using Microsoft.OpenApi.Models;
using Polly;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddControllers();
services.AddHttpContextAccessor();

services.AddDbContext<AccountContext>(options =>
    options.UseNpgsql(Environment.GetEnvironmentVariable("SQL_CONNECTION")));

services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var config = ConfigurationOptions.Parse("redis:6379", true);
    return ConnectionMultiplexer.Connect(config);
});

services.AddApplicationServices();
services.AddProjectServices();

services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

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

Console.WriteLine("START MIGRATION");

var retry = Policy
    .Handle<Exception>()
    .WaitAndRetry(10, retryAttempt =>
    {
        Console.WriteLine($"⏳ Retry {retryAttempt}...");
        return TimeSpan.FromSeconds(3);
    });

retry.Execute(() =>
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AccountContext>();
    db.Database.Migrate();
});

Console.WriteLine("END MIGRATION");
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();
app.Run();
