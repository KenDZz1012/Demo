namespace Account.Application.Features.UserRelationship.Queries.GetListFriendPending;

public class GetListFriendPendingVm
{
    public Guid Id { get; set; }
   
    public string UserName { get; set; }
   
    public string DisplayName { get; set; }
   
    public string AvatarUrl { get; set; }
   
    public bool IsSender { get; set; }
}