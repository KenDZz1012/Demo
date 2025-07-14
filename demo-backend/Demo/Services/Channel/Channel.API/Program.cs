using Channel.API.DependencyInjection;
using Channel.Application;
using Channel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Account.Grpc.Protos;
using Channel.Application.GrpcServices;

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