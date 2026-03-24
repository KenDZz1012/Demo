using System.ComponentModel.DataAnnotations.Schema;
using Channel.Domain.Common;

namespace Channel.Domain.Entities;

[Table("server_members")]
public partial class ServerMember : BaseEntity
{
    public Guid Id { get; set; }

    public Guid ServerId { get; set; }

    public Guid UserId { get; set; }

    /// <summary>Owner | Member</summary>
    public string? Role { get; set; }

    public DateTime? JoinedAt { get; set; }

    public virtual Server Server { get; set; } = null!;
}
