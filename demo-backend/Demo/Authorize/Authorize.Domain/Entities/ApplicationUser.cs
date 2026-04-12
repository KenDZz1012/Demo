using Microsoft.AspNetCore.Identity;

namespace Authorize.Domain.Entities;

/// <summary>
/// Người dùng đăng nhập cục bộ (ASP.NET Identity), được Duende IdentityServer dùng làm user store
/// khi phát hành token (ROPC, profile, v.v.).
/// </summary>
public class ApplicationUser : IdentityUser<Guid>
{
    public string DisplayName { get; set; }
    public string AvatarUrl { get; set; }
    public string AccountStatus { get; set; } = "active"; // active | banned | pending
}