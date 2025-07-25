using MediatR;
using Service.Lib.BaseResponse;

namespace Account.Application.Features.UserRelationship.Queries.GetListFriendPending;

public class GetListFriendPending : IRequest<ApiResponse<List<GetListFriendPendingVm>>>
{
    public Guid UserId { get; set; }
    
    public GetListFriendPending() { }

    public GetListFriendPending(Guid userId)
    {
        UserId = userId;
    }
}