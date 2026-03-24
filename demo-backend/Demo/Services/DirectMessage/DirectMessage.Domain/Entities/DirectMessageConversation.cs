using Cassandra.Mapping.Attributes;
using DirectMessage.Domain.Common;

namespace DirectMessage.Domain.Entities;

[Table("direct_message_conversations", Keyspace = "DirectMessage")]
public partial class DirectMessageConversation : BaseEntity
{
    [PartitionKey]
    [Column("conversation_id")]
    public Guid ConversationId { get; set; }

    [Column("name")]
    public string? Name { get; set; }

    /// <summary>Direct | Group</summary>
    [Column("type")]
    public string? Type { get; set; }
}
