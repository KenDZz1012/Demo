using Authorize.Application;
using Authorize.DependencyInjection;
using Authorize.Domain.Entities;
using Authorize.IdentityServer;
using Authorize.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Polly;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

// Host ASP.NET Identity (AuthorizeContext) + Duende IdentityServer (/connect/token, …). Chi tiết: IdentityServer/IdentityServerServiceExtensions.cs

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddHttpContextAccessor();
services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

services.AddDbContext<AuthorizeContext>(options =>
    options.UseNpgsql(Environment.GetEnvironmentVariable("SQL_CONNECTION")));

services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<AuthorizeContext>()
    .AddDefaultTokenProviders();

services.AddAuthorizeIdentityServer(builder.Environment);

services.AddApplicationServices();
services.AddProjectServices();

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
    var db = scope.ServiceProvider.GetRequiredService<AuthorizeContext>();
    db.Database.Migrate();
});

Console.WriteLine("END MIGRATION");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
