// Authorize.Domain/Entities/ApplicationUser.cs
using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    public string DisplayName { get; set; }
    public string AvatarUrl { get; set; }
    public string AccountStatus { get; set; } = "active"; // active | banned | pending
}