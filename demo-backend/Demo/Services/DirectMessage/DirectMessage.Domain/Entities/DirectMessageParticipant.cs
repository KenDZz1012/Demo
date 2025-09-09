using Cassandra.Mapping.Attributes;
namespace DirectMessage.Domain.Entities;

[Table("direct_message_participant", Keyspace = "DirectMessage")]
public partial class DirectMessageParticipant
{
    [Column("conversation_id")]
    public Guid ConversationId { get; set; }

    [Column("joined_at")]
    public DateTimeOffset JoinedAt { get; set; }

    [Column("role")]
    public string Role { get; set; }

    [Column("user_id")]
    public Guid UserId { get; set; }

}
