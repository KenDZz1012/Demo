using System.ComponentModel.DataAnnotations.Schema;
using Notification.Domain.Common;

namespace Notification.Domain.Entities;

[Table("user_notification_settings")]
public class UserNotificationSetting : BaseEntity
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    /// <summary>null = global setting</summary>
    public Guid? GuildId { get; set; }

    /// <summary>null = guild-level setting</summary>
    public Guid? ChannelId { get; set; }

    public DateTime? MutedUntil { get; set; }

    /// <summary>all | mentions | nothing</summary>
    public string MessageNotifications { get; set; } = "all";

    public bool SuppressEveryone { get; set; } = false;

    public bool SuppressRoles { get; set; } = false;

    public bool MobilePush { get; set; } = true;
}
