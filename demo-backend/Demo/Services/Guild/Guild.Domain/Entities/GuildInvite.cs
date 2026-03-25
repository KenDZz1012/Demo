using System.ComponentModel.DataAnnotations.Schema;
using Guild.Domain.Common;

namespace Guild.Domain.Entities;

[Table("guild_invites")]
public class GuildInvite : BaseEntity
{
    public Guid Id { get; set; }

    public string Code { get; set; } = null!;

    public Guid GuildId { get; set; }

    /// <summary>Channel the invite leads to</summary>
    public Guid? ChannelId { get; set; }

    public Guid? CreatorId { get; set; }

    public int MaxUses { get; set; } = 0;

    public int Uses { get; set; } = 0;

    public DateTime? ExpiresAt { get; set; }

    public virtual Guild Guild { get; set; } = null!;
}
