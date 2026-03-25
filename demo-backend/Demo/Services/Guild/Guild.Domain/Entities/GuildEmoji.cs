using System.ComponentModel.DataAnnotations.Schema;
using Guild.Domain.Common;

namespace Guild.Domain.Entities;

[Table("guild_emojis")]
public class GuildEmoji : BaseEntity
{
    public Guid Id { get; set; }

    public Guid GuildId { get; set; }

    public string Name { get; set; } = null!;

    public string Url { get; set; } = null!;

    public bool Animated { get; set; } = false;

    public bool Available { get; set; } = true;

    public virtual Guild Guild { get; set; } = null!;
}
