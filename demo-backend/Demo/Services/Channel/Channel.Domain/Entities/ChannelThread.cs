using System.ComponentModel.DataAnnotations.Schema;
using Channel.Domain.Common;

namespace Channel.Domain.Entities;

[Table("channel_threads")]
public class ChannelThread : BaseEntity
{
    public Guid Id { get; set; }

    public Guid ChannelId { get; set; }

    public Guid OwnerId { get; set; }

    public string Name { get; set; } = null!;

    public bool Archived { get; set; } = false;

    public bool Locked { get; set; } = false;

    /// <summary>Minutes until auto-archive: 60 | 1440 | 4320 | 10080</summary>
    public int AutoArchiveDuration { get; set; } = 1440;

    public virtual Channel Channel { get; set; } = null!;

    public virtual ICollection<ThreadMember> ThreadMembers { get; set; } = new List<ThreadMember>();
}
