using MediatR;
using Service.Lib.BaseResponse;

namespace Account.Application.Features.UserRelationship.Queries.GetListFriendQuery;

public class GetListFriend: IRequest<ApiResponse<List<GetListFriendVm>>>
{
    public Guid UserId { get; set; }
    
    public GetListFriend() { }

    public GetListFriend(Guid userId)
    {
        UserId = userId;
    }
}