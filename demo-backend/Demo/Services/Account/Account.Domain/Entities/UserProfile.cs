using System.ComponentModel.DataAnnotations.Schema;
using Account.Domain.Common;

namespace Account.Domain.Entities;

[Table("user_profiles")]
public class UserProfile : BaseEntity
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string? Bio { get; set; }

    public string? BannerUrl { get; set; }

    public string? Pronouns { get; set; }

    public virtual User User { get; set; } = null!;
}
