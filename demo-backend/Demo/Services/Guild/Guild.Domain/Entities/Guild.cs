using System.ComponentModel.DataAnnotations.Schema;
using Guild.Domain.Common;

namespace Guild.Domain.Entities;

[Table("guilds")]
public class Guild : BaseEntity
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? IconUrl { get; set; }

    public string? BannerUrl { get; set; }

    public Guid OwnerId { get; set; }

    /// <summary>None | Low | Medium | High | VeryHigh</summary>
    public string VerificationLevel { get; set; } = "None";

    public int MaxMembers { get; set; } = 500000;

    public virtual ICollection<GuildMember> GuildMembers { get; set; } = new List<GuildMember>();

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();

    public virtual ICollection<GuildBan> GuildBans { get; set; } = new List<GuildBan>();

    public virtual ICollection<GuildInvite> GuildInvites { get; set; } = new List<GuildInvite>();

    public virtual ICollection<GuildEmoji> GuildEmojis { get; set; } = new List<GuildEmoji>();
}
