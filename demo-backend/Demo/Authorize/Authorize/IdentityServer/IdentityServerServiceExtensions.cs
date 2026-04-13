using Authorize.Application.IdentityServer;
using Authorize.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Authorize.IdentityServer;

/// <summary>
/// Gom cấu hình Duende IdentityServer: kết nối ASP.NET Identity, store in-memory cho clients/scopes
/// (theo <see cref="Config"/>), custom profile, ROPC validator, và khóa ký JWT.
/// </summary>
public static class IdentityServerServiceExtensions
{
    /// <summary>
    /// Đăng ký đầy đủ pipeline IdentityServer cho service Authorize.
    /// </summary>
    public static IServiceCollection AddAuthorizeIdentityServer(
        this IServiceCollection services,
        IWebHostEnvironment environment)
    {
        // AddAspNetIdentity: tích hợp UserManager/SignInManager, đăng ký sẵn Resource Owner Password validator cho grant type password.
        var identityServer = services.AddIdentityServer(options =>
            {
                // Giúp access token có audience ổn định khi validate ở API resource (tùy cấu hình resource).
                options.EmitStaticAudienceClaim = true;
            })
            .AddAspNetIdentity<ApplicationUser>()
            .AddInMemoryIdentityResources(Config.IdentityResources)
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryApiResources(Config.ApiResources)
            .AddInMemoryClients(Config.Clients)
            .AddProfileService<ProfileService>();

        // Khóa ký token (JWT). Development: file temp có persist. Production: nên thay bằng certificate / vault.
        if (environment.IsDevelopment())
            identityServer.AddDeveloperSigningCredential(persistKey: true);
        else
            identityServer.AddDeveloperSigningCredential();

        return services;
    }
}
