using Cassandra.Mapping;
using Cassandra.Mapping.Attributes;
using DirectMessage.Domain.Common;

namespace DirectMessage.Domain.Entities;

[Table("direct_messages", Keyspace = "DirectMessage")]
public partial class DirectMessage : BaseEntity
{
    [PartitionKey]
    [Column("conversation_id")]
    public Guid ConversationId { get; set; }

    [ClusteringKey(0, SortOrder.Descending)]
    [Column("message_id")]
    public string MessageId { get; set; } = null!;

    [Column("sender_id")]
    public Guid SenderId { get; set; }

    [Column("content")]
    public string? Content { get; set; }
}
