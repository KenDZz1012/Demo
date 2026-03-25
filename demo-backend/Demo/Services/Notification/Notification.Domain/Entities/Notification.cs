using System.ComponentModel.DataAnnotations.Schema;
using Notification.Domain.Common;

namespace Notification.Domain.Entities;

[Table("notifications")]
public class Notification : BaseEntity
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    /// <summary>mention | reply | friend_request | guild_invite | system</summary>
    public string Type { get; set; } = null!;

    public string? Title { get; set; }

    public string? Body { get; set; }

    /// <summary>message | guild | user | channel</summary>
    public string? ReferenceType { get; set; }

    public Guid? ReferenceId { get; set; }

    public bool IsRead { get; set; } = false;
}
