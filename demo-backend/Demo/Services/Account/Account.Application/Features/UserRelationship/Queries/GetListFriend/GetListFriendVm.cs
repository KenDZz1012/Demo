namespace Account.Application.Features.UserRelationship.Queries.GetListFriendQuery;

public class GetListFriendVm
{
   public Guid Id { get; set; }
   
   public string UserName { get; set; }
   
   public string DisplayName { get; set; }
   
   public string AvatarUrl { get; set; }
   
   public bool IsOnline { get; set; }
}