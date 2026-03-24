using System.ComponentModel.DataAnnotations.Schema;
using Account.Domain.Common;
using Account.Domain.Common.Constants;

namespace Account.Domain.Entities;

[Table("user_relationships")]
public partial class UserRelationship : BaseEntity
{
    public Guid Id { get; set; }

    public Guid RequesterId { get; set; }

    public Guid AddresseeId { get; set; }

    /// <summary>Pending | Accepted | Blocked</summary>
    public string Status { get; set; } = UserRelationshipStatus.Pending;

    public virtual User Addressee { get; set; } = null!;

    public virtual User Requester { get; set; } = null!;
}
