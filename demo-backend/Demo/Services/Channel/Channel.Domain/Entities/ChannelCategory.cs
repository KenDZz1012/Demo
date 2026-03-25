using System.ComponentModel.DataAnnotations.Schema;
using Channel.Domain.Common;

namespace Channel.Domain.Entities;

[Table("channel_categories")]
public class ChannelCategory : BaseEntity
{
    public Guid Id { get; set; }

    /// <summary>References guilds.id in Guild service (no FK constraint — cross-service)</summary>
    public Guid GuildId { get; set; }

    public string Name { get; set; } = null!;

    public int Position { get; set; } = 0;

    public virtual ICollection<Channel> Channels { get; set; } = new List<Channel>();
}
