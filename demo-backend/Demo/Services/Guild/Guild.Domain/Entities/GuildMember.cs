using System.ComponentModel.DataAnnotations.Schema;
using Guild.Domain.Common;

namespace Guild.Domain.Entities;

[Table("guild_members")]
public class GuildMember : BaseEntity
{
    public Guid Id { get; set; }

    public Guid GuildId { get; set; }

    public Guid UserId { get; set; }

    public string? Nickname { get; set; }

    public string? AvatarUrl { get; set; }

    public DateTime? JoinedAt { get; set; }

    public bool IsMuted { get; set; } = false;

    public bool IsDeafened { get; set; } = false;

    public virtual Guild Guild { get; set; } = null!;

    public virtual ICollection<MemberRole> MemberRoles { get; set; } = new List<MemberRole>();
}
