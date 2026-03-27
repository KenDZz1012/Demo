using Duende.IdentityServer.Models;

namespace Authorize.IdentityServer;

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
        new ApiScope("notification.api"),   // ← thêm
        new ApiScope("media.api"),          // ← thêm
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
            Scopes = { "api.read", "api.write", "guild.api",
                "guild.create", "guild.delete", "guild.manage" }
        },
        new ApiResource("channel.api", "Channel Service")
        {
            Scopes = { "api.read", "api.write", "channel.api",
                "channel.create", "channel.delete" }
        },
        new ApiResource("message.api", "Message Service")
        {
            Scopes = { "api.read", "api.write", "message.api",
                "message.delete_others" }  // ← thêm
        },
        new ApiResource("notification.api", "Notification Service") // ← thêm
        {
            Scopes = { "api.read", "api.write", "notification.api" }
        },
        new ApiResource("media.api", "Media Service") // ← thêm
        {
            Scopes = { "api.read", "api.write", "media.api" }
        },
        new ApiResource("directmessage.api", "DirectMessage Service") // ← thêm
        {
            Scopes = { "api.read", "api.write", "directmessage.api" }
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