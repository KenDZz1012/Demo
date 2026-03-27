namespace Servivce.HttpHelper.Dtos.Account;

public partial class UserHttpDto
{
    public Guid Id { get; set; }

    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? AvatarUrl { get; set; }

    public string DisplayName { get; set; } = null!;

    public DateOnly? DateOfBirth { get; set; }
}