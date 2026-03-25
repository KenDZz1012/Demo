using System.ComponentModel.DataAnnotations.Schema;
using Channel.Domain.Common;

namespace Channel.Domain.Entities;

[Table("channels")]
public partial class Channel : BaseEntity
{
    public Guid Id { get; set; }

    /// <summary>References guilds.id in Guild service (no FK constraint — cross-service)</summary>
    public Guid GuildId { get; set; }

    public Guid? CategoryId { get; set; }

    public string? Name { get; set; }

    /// <summary>Text | Voice | Announcement | Stage | Forum</summary>
    public string? Type { get; set; }

    public int Position { get; set; } = 0;

    public string? Topic { get; set; }

    public bool Nsfw { get; set; } = false;

    /// <summary>Slowmode in seconds (0 = disabled)</summary>
    public int RateLimit { get; set; } = 0;

    /// <summary>Bitrate for voice channels (bps)</summary>
    public int? Bitrate { get; set; }

    /// <summary>Max users for voice channels (0 = unlimited)</summary>
    public int? UserLimit { get; set; }

    public virtual ChannelCategory? Category { get; set; }

    public virtual ICollection<ChannelPermissionOverride> PermissionOverrides { get; set; } = new List<ChannelPermissionOverride>();

    public virtual ICollection<ChannelThread> Threads { get; set; } = new List<ChannelThread>();
}
