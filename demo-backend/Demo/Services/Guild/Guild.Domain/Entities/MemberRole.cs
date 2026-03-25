using System.ComponentModel.DataAnnotations.Schema;
using Guild.Domain.Common;

namespace Guild.Domain.Entities;

[Table("member_roles")]
public class MemberRole : BaseEntity
{
    public Guid Id { get; set; }

    public Guid MemberId { get; set; }

    public Guid RoleId { get; set; }

    public virtual GuildMember Member { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;
}
