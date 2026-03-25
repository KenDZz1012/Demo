using System.ComponentModel.DataAnnotations.Schema;
using Channel.Domain.Common;

namespace Channel.Domain.Entities;

[Table("channel_permission_overrides")]
public class ChannelPermissionOverride : BaseEntity
{
    public Guid Id { get; set; }

    public Guid ChannelId { get; set; }

    /// <summary>role | member</summary>
    public string TargetType { get; set; } = null!;

    /// <summary>RoleId or UserId depending on TargetType</summary>
    public Guid TargetId { get; set; }

    /// <summary>Bitfield of explicitly allowed permissions</summary>
    public long Allow { get; set; } = 0;

    /// <summary>Bitfield of explicitly denied permissions</summary>
    public long Deny { get; set; } = 0;

    public virtual Channel Channel { get; set; } = null!;
}
