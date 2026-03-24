using Cassandra.Mapping.Attributes;
using DirectMessage.Domain.Common;

namespace DirectMessage.Domain.Entities;

[Table("direct_message_participants", Keyspace = "DirectMessage")]
public partial class DirectMessageParticipant : BaseEntity
{
    [PartitionKey]
    [Column("conversation_id")]
    public Guid ConversationId { get; set; }

    [ClusteringKey]
    [Column("user_id")]
    public Guid UserId { get; set; }

    /// <summary>Owner | Member</summary>
    [Column("role")]
    public string? Role { get; set; }

    [Column("joined_at")]
    public DateTimeOffset? JoinedAt { get; set; }
}
