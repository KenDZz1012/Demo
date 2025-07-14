using Account.Application.Contracts.Persistence;
using Account.Grpc.Services;
using Account.Infrastructure.Data;
using Account.Infrastructure.Repositories;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(80, o => o.Protocols = HttpProtocols.Http2);
});
services.AddAutoMapper(Assembly.GetExecutingAssembly());

services.AddDbContext<AccountContext>(options =>
    options.UseNpgsql(Environment.GetEnvironmentVariable("SQL_CONNECTION")));

services.AddScoped<IUserRepository, UserRepository>();
services.AddGrpc();

var app = builder.Build();

// Đặt AppContext Switch cho phép HTTP/2 over plaintext (nếu client dùng http)
AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

// ❌ KHÔNG gọi app.Run() trước UseRouting và UseEndpoints

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<UserService>();

    endpoints.MapGet("/", async context =>
    {
        await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
    });
});

app.Run(); // ✅ Đây phải là dòng cuối cùng
