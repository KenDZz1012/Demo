using System.ComponentModel.DataAnnotations.Schema;
using Channel.Domain.Common;

namespace Channel.Domain.Entities;

[Table("thread_members")]
public class ThreadMember : BaseEntity
{
    public Guid Id { get; set; }

    public Guid ThreadId { get; set; }

    public Guid UserId { get; set; }

    public DateTime? JoinedAt { get; set; }

    public virtual ChannelThread Thread { get; set; } = null!;
}
