using Channel.API.DependencyInjection;
using Channel.Application;
using Channel.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using Polly;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, configuration) =>
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext()
        .Enrich.WithMachineName()
        .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
        .WriteTo.Console()
        .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(
            new Uri(context.Configuration["ElasticConfiguration:Uri"] ?? "http://elasticsearch:9200"))
        {
            IndexFormat = $"channel-api-logs-{DateTime.UtcNow:yyyy-MM}",
            AutoRegisterTemplate = true,
            AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
            NumberOfShards = 2,
            NumberOfReplicas = 1,
            EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog | EmitEventFailureHandling.RaiseCallback,
            FailureCallback = (e, ex) => Console.WriteLine($"[Serilog-ES] Failed: {e.MessageTemplate} | {ex?.Message}")
        })
);
ConfigureServices(builder.Services, builder.Configuration);


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
    var db = scope.ServiceProvider.GetRequiredService<ChannelContext>();
    db.Database.Migrate();
});

Console.WriteLine("END MIGRATION");

ConfigureMiddleware(app);

app.Run();

void ConfigureServices(IServiceCollection services, ConfigurationManager configuration)
{
    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.Authority = configuration["Auth:Authority"]
                                ?? Environment.GetEnvironmentVariable("AUTHORIZE_URL")
                                ?? "http://authorize.api:80";
            options.RequireHttpsMetadata = bool.TryParse(configuration["Auth:RequireHttpsMetadata"], out var requireHttps)
                ? requireHttps
                : false;
            options.MapInboundClaims = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidAudience = "channel.api",
                NameClaimType = "name",
                RoleClaimType = "role"
            };
        });
    services.AddAuthorization(options =>
    {
        options.AddPolicy("Channel.Read", policy => policy.RequireAuthenticatedUser().RequireScope("channel.api"));
        options.AddPolicy("Channel.Create", policy => policy.RequireAuthenticatedUser().RequireScope("channel.create"));
        options.AddPolicy("Channel.Delete", policy => policy.RequireAuthenticatedUser().RequireScope("channel.delete"));
    });

    services.AddApplicationServices();
    services.AddProjectServices();

    var connectionString = Environment.GetEnvironmentVariable("SQL_CONNECTION");
    services.AddDbContext<ChannelContext>(options =>
        options.UseNpgsql(connectionString, sqlOptions => { sqlOptions.EnableRetryOnFailure(); }));

    services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
    });
    
    services.AddSwaggerGen();
}

void ConfigureMiddleware(WebApplication app)
{
    app.UseSerilogRequestLogging();

    if (app.Environment.IsDevelopment() || app.Environment.IsStaging() || app.Environment.IsProduction())
    {
        app.UseSwagger();

        app.UseSwaggerUI();
    }

    app.UseCors("AllowAll");

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
}

static class AuthorizationPolicyBuilderExtensions
{
    public static AuthorizationPolicyBuilder RequireScope(this AuthorizationPolicyBuilder builder, string scope)
    {
        return builder.RequireAssertion(context =>
            context.User.Claims
                .Where(c => c.Type == "scope")
                .SelectMany(c => c.Value.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                .Contains(scope, StringComparer.Ordinal));
    }
}