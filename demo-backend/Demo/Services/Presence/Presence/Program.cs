using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Presence.Hubs;
using Presence.Services;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var config = ConfigurationOptions.Parse("redis:6379", true);
    return ConnectionMultiplexer.Connect(config);
});
builder.Services.AddSingleton<IConnectionManager, RedisConnectionManager>();

// 4. Cấu hình JWT Authentication (nhận từ cookie hoặc query)
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
            )
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                // Ưu tiên access_token từ query (SignalR WebSocket)
                var accessToken = context.Request.Query["access_token"];

                if (string.IsNullOrEmpty(accessToken))
                {
                    // Nếu không có thì lấy từ cookie
                    var cookieToken = context.Request.Cookies["access_token"];
                    if (!string.IsNullOrEmpty(cookieToken))
                        context.Token = cookieToken;
                }
                else
                {
                    context.Token = accessToken;
                }

                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
// Add services to the container.

var app = builder.Build();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
// Configure the HTTP request pipeline.
app.MapHub<PresenceHub>("/presence");

app.Run();

