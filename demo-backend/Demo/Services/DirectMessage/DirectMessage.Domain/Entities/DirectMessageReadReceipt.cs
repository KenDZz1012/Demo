using Cassandra.Mapping.Attributes;
namespace DirectMessage.Domain.Entities;

[Table("direct_message_read_receipt", Keyspace = "DirectMessage")]
public partial class DirectMessageReadReceipt
{
    [Column("conversation_id")]
    public Guid ConversationId { get; set; }

    [Column("last_read_message_id")]
    public string LastReadMessageId { get; set; }

    [Column("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; }

    [Column("user_id")]
    public Guid UserId { get; set; }

}
