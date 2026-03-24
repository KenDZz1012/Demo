using System.ComponentModel.DataAnnotations.Schema;
using Channel.Domain.Common;

namespace Channel.Domain.Entities;

[Table("server_invite_links")]
public partial class ServerInviteLink : BaseEntity
{
    public Guid Id { get; set; }

    public Guid ServerId { get; set; }

    public string Code { get; set; } = null!;

    public DateTime? ExpiresAt { get; set; }

    public virtual Server Server { get; set; } = null!;
}
