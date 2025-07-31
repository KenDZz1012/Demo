namespace Presence.Models;

public class FriendRequestPayload
{
    public string FromUserName { get; set; }
    public string FromUserId { get; set; }
    
    public string FromUserAvatarUrl { get; set; }

    public string FromUserDisplayName { get; set; }
    
    public string ToUserName { get; set; }
}