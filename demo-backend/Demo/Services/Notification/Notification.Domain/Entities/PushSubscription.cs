using System.ComponentModel.DataAnnotations.Schema;
using Notification.Domain.Common;

namespace Notification.Domain.Entities;

[Table("push_subscriptions")]
public class PushSubscription : BaseEntity
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Endpoint { get; set; } = null!;

    public string P256dhKey { get; set; } = null!;

    public string AuthKey { get; set; } = null!;

    /// <summary>web | android | ios</summary>
    public string? Platform { get; set; }
}
