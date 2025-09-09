using Cassandra.Mapping.Attributes;

namespace DirectMessage.Domain.Entities;

[Table("direct_message", Keyspace = "DirectMessage")]
public partial class DirectMessage
{
    [Column("content")]
    public string Content { get; set; }

    [Column("conversation_id")]
    public Guid ConversationId { get; set; }

    [Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    [Column("message_id")]
    public string MessageId { get; set; }

    [Column("sender_id")]
    public Guid SenderId { get; set; }

}
