using Channel.API.DependencyInjection;
using Channel.Application;
using Channel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Service.Lib.HttpRequest;

var builder = WebApplication.CreateBuilder(args);

// Register Services
ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

// Configure Middleware Pipeline
ConfigureMiddleware(app);

app.Run();

void ConfigureServices(IServiceCollection services, ConfigurationManager configuration)
{
    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    // Custom application services
    services.AddApplicationServices();
    services.AddProjectServices();

    // Database
    var connectionString = Environment.GetEnvironmentVariable("SQL_CONNECTION");
    services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(connectionString));
    
    // CORS
    services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
    });
}

void ConfigureMiddleware(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseCors("AllowAll");

    app.UseAuthorization();

    app.MapControllers();
}