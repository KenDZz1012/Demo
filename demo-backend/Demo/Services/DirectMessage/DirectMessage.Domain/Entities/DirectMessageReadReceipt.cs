using Cassandra.Mapping.Attributes;
using DirectMessage.Domain.Common;

namespace DirectMessage.Domain.Entities;

[Table("direct_message_read_receipts", Keyspace = "DirectMessage")]
public partial class DirectMessageReadReceipt : BaseEntity
{
    [PartitionKey]
    [Column("conversation_id")]
    public Guid ConversationId { get; set; }

    [ClusteringKey]
    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("last_read_message_id")]
    public string? LastReadMessageId { get; set; }
}
