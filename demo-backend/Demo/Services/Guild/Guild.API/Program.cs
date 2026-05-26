using Guild.API.DependencyInjection;
using Guild.Application;
using Guild.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Polly;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Auth:Authority"]
                            ?? Environment.GetEnvironmentVariable("AUTHORIZE_URL")
                            ?? "http://authorize.api:80";
        options.RequireHttpsMetadata = bool.TryParse(builder.Configuration["Auth:RequireHttpsMetadata"], out var requireHttps)
            ? requireHttps
            : false;
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidAudience = "guild.api",
            NameClaimType = "name",
            RoleClaimType = "role"
        };
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Guild.Read", policy => policy.RequireAuthenticatedUser().RequireScope("guild.api"));
    options.AddPolicy("Guild.Create", policy => policy.RequireAuthenticatedUser().RequireScope("guild.create"));
    options.AddPolicy("Guild.Delete", policy => policy.RequireAuthenticatedUser().RequireScope("guild.delete"));
    options.AddPolicy("Guild.Manage", policy => policy.RequireAuthenticatedUser().RequireScope("guild.manage"));
});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Guild API",
        Version = "v1"
    });
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

builder.Services.AddDbContext<GuildContext>(options =>
    options.UseNpgsql(Environment.GetEnvironmentVariable("SQL_CONNECTION")));

builder.Services.AddApplicationServices();
builder.Services.AddProjectServices();

builder.Services.AddCors(options =>
    options.AddPolicy("AllowAll", p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

var app = builder.Build();

Console.WriteLine("START MIGRATION");

var retry = Policy
    .Handle<Exception>()
    .WaitAndRetry(10, attempt =>
    {
        Console.WriteLine($"⏳ Retry {attempt}...");
        return TimeSpan.FromSeconds(3);
    });

retry.Execute(() =>
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<GuildContext>();
    db.Database.Migrate();
});

Console.WriteLine("END MIGRATION");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

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
