using System.ComponentModel.DataAnnotations.Schema;
using Guild.Domain.Common;

namespace Guild.Domain.Entities;

[Table("guild_bans")]
public class GuildBan : BaseEntity
{
    public Guid Id { get; set; }

    public Guid GuildId { get; set; }

    public Guid UserId { get; set; }

    public string? Reason { get; set; }

    public virtual Guild Guild { get; set; } = null!;
}
