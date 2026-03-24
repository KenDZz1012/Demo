using System.ComponentModel.DataAnnotations.Schema;
using Channel.Domain.Common;

namespace Channel.Domain.Entities;

[Table("servers")]
public partial class Server : BaseEntity
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public Guid OwnerId { get; set; }

    public string? IconUrl { get; set; }

    public virtual ICollection<Channel> Channels { get; set; } = new List<Channel>();

    public virtual ICollection<ServerMember> ServerMembers { get; set; } = new List<ServerMember>();

    public virtual ICollection<ServerInviteLink> ServerInviteLinks { get; set; } = new List<ServerInviteLink>();
}
