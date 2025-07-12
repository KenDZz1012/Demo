using Channel.API.DependencyInjection;
using Channel.Application;
using Channel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Account.Grpc.Protos;
using Channel.Application.GrpcServices;

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
        options.UseSqlServer(connectionString, sqlOptions => { sqlOptions.EnableRetryOnFailure(); }));

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


    // gRPC client
    services.AddGrpcClient<AccountProtoSerivce.AccountProtoSerivceClient>(o =>
        o.Address = new Uri("http://account.grpc:80"));

    services.AddScoped<UserGrpcService>();
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