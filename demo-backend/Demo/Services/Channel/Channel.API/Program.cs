using Channel.API.DependencyInjection;
using Channel.Application;
using Channel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Account.Grpc.Protos;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using Polly;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, configuration) =>
    configuration.Enrich.FromLogContext().Enrich.WithMachineName().WriteTo.Console().WriteTo.Elasticsearch(
            new ElasticsearchSinkOptions(
                new Uri(context.Configuration["ElasticConfiguration:Uri"] ?? "http://localhost:9200")
            )
            {
                IndexFormat = $"channel-api-logs-{DateTime.UtcNow:yyyy-MM}",
                AutoRegisterTemplate = true,
                NumberOfShards = 2,
                NumberOfReplicas = 1
            }).Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName).ReadFrom
        .Configuration(context.Configuration)
);
ConfigureServices(builder.Services, builder.Configuration);


var app = builder.Build();
Console.WriteLine("🔥 START MIGRATION");

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

Console.WriteLine("✅ END MIGRATION");

ConfigureMiddleware(app);

app.Run();

void ConfigureServices(IServiceCollection services, ConfigurationManager configuration)
{
    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

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

    services.AddGrpcClient<AccountProtoSerivce.AccountProtoSerivceClient>(o =>
        o.Address = new Uri("http://account.grpc:80"));

    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "My API",
            Version = "v1"
        });
        c.AddServer(new OpenApiServer { Url = "/cha" });
        c.AddServer(new OpenApiServer { Url = "/" });
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "Enter JWT Bearer token **_only_** (without 'Bearer ' prefix)",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT"
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    });
}

void ConfigureMiddleware(WebApplication app)
{
    if (app.Environment.IsDevelopment() || app.Environment.IsStaging() || app.Environment.IsProduction())
    {
        app.UseSwagger();

        app.UseSwaggerUI();
    }

    app.UseCors("AllowAll");

    app.UseAuthorization();

    app.MapControllers();
}