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

// Add Controllers
services.AddControllers();

// Configure Database
services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(Environment.GetEnvironmentVariable("SQL_CONNECTION")));

// Application & Infrastructure Layer
services.AddApplicationServices();
services.AddProjectServices(); // Your custom DI setup from API layer

// External Services
services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
services.AddSingleton<MinioContext>();
services.AddSingleton<MinioService>();

// Keycloak HTTP Client (with bypass SSL for dev)
services.AddHttpClient<KeycloakService>();
services.AddHttpClient<IKeycloakService, KeycloakService>()
        .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
        });

// Swagger
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Account API", Version = "v1" });
});

// CORS - Allow All (customize if needed)
services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

var app = builder.Build();

// Configure Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();
app.Run();
