using Account.Grpc.Protos;
using Authorize.Application;
using Authorize.DependencyInjection;
using Authorize.GrpcServices;
using Authorize.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Service.Lib.Keycloak;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

// Add services to the container
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

// Database configuration
services.AddDbContext<AuthorizeContext>(options =>
    options.UseSqlServer(Environment.GetEnvironmentVariable("SQL_CONNECTION")));

// Application layer services
services.AddApplicationServices();
services.AddProjectServices();

// gRPC client
services.AddGrpcClient<AccountProtoSerivce.AccountProtoSerivceClient>(o =>
    o.Address = new Uri("http://account.grpc:80"));

services.AddScoped<UserGrpcService>();

// Configure Keycloak HTTP client with custom handler
services.AddHttpClient<IKeycloakService, KeycloakService>()
    .ConfigurePrimaryHttpMessageHandler(() =>
        new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (_, _, _, _) => true
        });

var app = builder.Build();

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();
app.Run();
