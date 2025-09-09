using Cassandra.Mapping.Attributes;
namespace DirectMessage.Domain.Entities;

[Table("direct_message_conversation", Keyspace = "DirectMessage")]
public partial class DirectMessageConversation
{
    [Column("conversation_id")]
    public Guid ConversationId { get; set; }

    [Column("created_at")]
    public DateTimeOffset CreatedAt { get; set; }

    [Column("name")]
    public string Name { get; set; }

    [Column("type")]
    public string Type { get; set; }

}
