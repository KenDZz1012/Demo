using Account.Grpc.Context;
using Account.Grpc.Repositories;
using Account.Grpc.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddDbContext<UserContext>(options =>
    options.UseSqlServer(Environment.GetEnvironmentVariable("SQL_CONNECTION")));

services.AddScoped<IUserRepository, UserRepository>();
// Add services to the container.
services.AddGrpc();


var app = builder.Build();

// Configure the HTTP request pipeline.



app.Run();
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<UserService>();

    endpoints.MapGet("/", async context =>
    {
        await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
    });
});