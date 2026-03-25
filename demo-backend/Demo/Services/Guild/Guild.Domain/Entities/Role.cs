using System.ComponentModel.DataAnnotations.Schema;
using Guild.Domain.Common;

namespace Guild.Domain.Entities;

[Table("roles")]
public class Role : BaseEntity
{
    public Guid Id { get; set; }

    public Guid GuildId { get; set; }

    public string Name { get; set; } = null!;

    /// <summary>Hex color code, e.g. #FF5733</summary>
    public string? Color { get; set; }

    /// <summary>Bitfield of permissions</summary>
    public long Permissions { get; set; } = 0;

    public int Position { get; set; } = 0;

    /// <summary>Display separately in member list</summary>
    public bool Hoist { get; set; } = false;

    /// <summary>Can be @mentioned</summary>
    public bool Mentionable { get; set; } = false;

    public virtual Guild Guild { get; set; } = null!;

    public virtual ICollection<MemberRole> MemberRoles { get; set; } = new List<MemberRole>();
}
