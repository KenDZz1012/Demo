namespace Presence.Models;

public class FriendRequestRejectedPayload
{
    public string? FromUserName { get; set; }
    public string? FromUserId { get; set; }
    public string? ToUserName { get; set; }
}