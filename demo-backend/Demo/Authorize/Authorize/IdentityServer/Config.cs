using Duende.IdentityServer.Models;

namespace Authorize.IdentityServer;

/// <summary>
/// Cấu hình tĩnh IdentityServer (in-memory): identity resources (openid, profile, roles),
/// API scopes/resources (audience cho từng microservice), và OAuth clients (web ROPC + machine client_credentials).
/// Dữ liệu được nạp qua <see cref="IdentityServerServiceExtensions.AddAuthorizeIdentityServer"/>.
/// Biến môi trường WEB_CLIENT_SECRET / SERVICE_CLIENT_SECRET phải có giá trị (hash SHA256 vào client secret).
/// </summary>
public class Config
{
    public static IEnumerable<IdentityResource> IdentityResources => new[]
    {
        new IdentityResources.OpenId(),   // sub (userId) — bắt buộc
        new IdentityResources.Profile(),  // name, website...
        new IdentityResource("roles", "User Roles", new[] { "role" }) // custom: nhét role vào token
    };
    
    public static IEnumerable<ApiScope> ApiScopes => new[]
    {
        new ApiScope("api.read",  "Read access"),
        new ApiScope("api.write", "Write access"),
        new ApiScope("account.api"),
        new ApiScope("guild.api"),
        new ApiScope("channel.api"),
        new ApiScope("message.api"),
        new ApiScope("notification.api"),
        new ApiScope("media.api"),         
        new ApiScope("directmessage.api"),
        
        new ApiScope("guild.create",  "Create guild"),
        new ApiScope("guild.delete",  "Delete guild"),
        new ApiScope("guild.manage",  "Manage guild members"),
        
        new ApiScope("channel.create", "Create channel"),
        new ApiScope("channel.delete", "Delete channel"),
        new ApiScope("message.delete_others", "Delete other's messages"), // moderator

    };
    
    public static IEnumerable<ApiResource> ApiResources => new[]
    {
        new ApiResource("account.api", "Account Service")
        {
            Scopes = { "api.read", "api.write", "account.api" }
        },
        new ApiResource("guild.api", "Guild Service")
        {
            Scopes = {  "guild.api",
                "guild.create", "guild.delete", "guild.manage" }
        },
        new ApiResource("channel.api", "Channel Service")
        {
            Scopes = {"channel.api",
                "channel.create", "channel.delete" }
        },
        new ApiResource("message.api", "Message Service")
        {
            Scopes = { "message.api",
                "message.delete_others" }  // ← thêm
        },
        new ApiResource("notification.api", "Notification Service") // ← thêm
        {
            Scopes = { "notification.api" }
        },
        new ApiResource("media.api", "Media Service") // ← thêm
        {
            Scopes = { "media.api" }
        },
        new ApiResource("directmessage.api", "DirectMessage Service") // ← thêm
        {
            Scopes = { "directmessage.api" }
        },
    };

    public static IEnumerable<Client> Clients => new[]
    {
        new Client
        {
            ClientId = "web-client",
            ClientSecrets = { new Secret(Environment.GetEnvironmentVariable("WEB_CLIENT_SECRET").Sha256()) },
            AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
            AllowOfflineAccess = true,
            AccessTokenLifetime = 3600,
            RefreshTokenExpiration = TokenExpiration.Sliding,
            AllowedScopes =
            {
                "openid", "profile", "roles",
                "api.read", "api.write",
                // service-level — để có aud trong token
                "guild.api", "channel.api", "message.api",
                "notification.api", "media.api", "directmessage.api",
                // action-level
                "guild.create", "guild.delete", "guild.manage",
                "channel.create", "channel.delete",
                "message.delete_others"
            }
        },
        new Client
        {
            ClientId = "service-client",
            ClientSecrets = { new Secret(Environment.GetEnvironmentVariable("SERVICE_CLIENT_SECRET").Sha256()) },
            AllowedGrantTypes = GrantTypes.ClientCredentials,
            AllowedScopes =
            {
                "account.api", "guild.api", "channel.api",
                "message.api", "notification.api", "media.api", "directmessage.api"
            }
        }
    };
}