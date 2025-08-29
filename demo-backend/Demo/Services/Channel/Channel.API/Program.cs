using Channel.API.DependencyInjection;
using Channel.Application;
using Channel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Account.Grpc.Protos;
using Channel.Application.GrpcServices;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

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

    services.AddScoped<UserGrpcService>();
    
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