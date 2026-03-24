using System.ComponentModel.DataAnnotations.Schema;
using Account.Domain.Common;
using Account.Domain.Common.Constants;
using Microsoft.EntityFrameworkCore;

namespace Account.Domain.Entities;

[Table("users")]
[Index("Email", Name = "uq_users_email", IsUnique = true)]
[Index("UserName", Name = "uq_users_username", IsUnique = true)]
public partial class User : BaseEntity
{
    public Guid Id { get; set; }

    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? AvatarUrl { get; set; }

    /// <summary>Active | Inactive | Banned</summary>
    public string Status { get; set; } = UserStatus.Active;

    public string DisplayName { get; set; } = null!;

    public DateOnly? DateOfBirth { get; set; }

    public bool IsAdmin { get; set; } = false;

    public virtual UserProfile? Profile { get; set; }

    public virtual UserSetting? Setting { get; set; }

    public virtual ICollection<UserRelationship> UserRelationshipAddressees { get; set; } = new List<UserRelationship>();

    public virtual ICollection<UserRelationship> UserRelationshipRequesters { get; set; } = new List<UserRelationship>();
}
