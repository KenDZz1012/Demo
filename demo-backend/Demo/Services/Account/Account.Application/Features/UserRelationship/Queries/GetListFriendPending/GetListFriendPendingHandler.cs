using Account.Application.Contracts.Persistence;
using AutoMapper;
using MediatR;
using Service.Lib.BaseResponse;

namespace Account.Application.Features.UserRelationship.Queries.GetListFriendPending;

public class GetListFriendPendingHandler : IRequestHandler<GetListFriendPending, ApiResponse<List<GetListFriendPendingVm>>>
{
    private readonly IUserRelationshipRepository _userRelationshipRepository;
    private readonly IMapper _mapper;

    public GetListFriendPendingHandler(IUserRelationshipRepository userRelationshipRepository, IMapper mapper)
    {
        _userRelationshipRepository = userRelationshipRepository;
        _mapper = mapper;
    }
    
    public async Task<ApiResponse<List<GetListFriendPendingVm>>> Handle(GetListFriendPending request, CancellationToken cancellationToken)
    {
        try
        {
            var userRelationships = await _userRelationshipRepository.GetUserRelationshipsPending(request.UserId);

            var friends = userRelationships.Select(x =>
            {
                var friend = x.RequesterId == request.UserId ? x.Addressee : x.Requester;
                return _mapper.Map<GetListFriendPendingVm>(friend);
            }).ToList();

            foreach (var friend in friends)
            {
                friend.IsSender = userRelationships.Any(x=> x.RequesterId == friend.Id);
            }
            return ApiResponse<List<GetListFriendPendingVm>>.Success(friends);
        }
        catch (Exception ex)
        {
            return ApiResponse<List<GetListFriendPendingVm>>.Failure("500", ex.Message);
        }
    }
}