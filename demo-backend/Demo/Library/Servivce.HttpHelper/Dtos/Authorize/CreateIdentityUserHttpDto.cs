namespace Servivce.HttpHelper.Dtos.Authorize;

public class CreateIdentityUserHttpDto
{
    public Guid? UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? DisplayName { get; set; }

    public string? AvatarUrl { get; set; }

    public string AccountStatus { get; set; } = "active"; 
}