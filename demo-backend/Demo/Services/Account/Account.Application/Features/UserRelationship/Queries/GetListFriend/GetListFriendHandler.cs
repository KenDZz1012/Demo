using Account.Application.Contracts.Persistence;
using AutoMapper;
using MediatR;
using Service.Lib.BaseResponse;

namespace Account.Application.Features.UserRelationship.Queries.GetListFriendQuery;

public class GetListFriendHandler : IRequestHandler<GetListFriend, ApiResponse<List<GetListFriendVm>>>
{
    private readonly IUserRelationshipRepository _userRelationshipRepository;
    private readonly IMapper _mapper;

    public GetListFriendHandler(IUserRelationshipRepository userRelationshipRepository, IMapper mapper)
    {
        _userRelationshipRepository = userRelationshipRepository;
        _mapper = mapper;
    }
    
    public async Task<ApiResponse<List<GetListFriendVm>>> Handle(GetListFriend request, CancellationToken cancellationToken)
    {
        try
        {
            var userRelationships = await _userRelationshipRepository.GetUserRelationships(request.UserId);

            var friends = userRelationships.Select(x =>
            {
                var friend = x.RequesterId == request.UserId ? x.Addressee : x.Requester;
                return _mapper.Map<GetListFriendVm>(friend);
            }).ToList();

            var friendIds = friends.Select(f => f.UserName).ToList();
            var onlineStatuses = await GetOnlineStatusesAsync(friendIds);

            foreach (var friend in friends)
            {
                friend.IsOnline = onlineStatuses.TryGetValue(friend.UserName, out var isOnline) && isOnline;
            }

            return ApiResponse<List<GetListFriendVm>>.Success(friends);
        }
        catch (Exception ex)
        {
            return ApiResponse<List<GetListFriendVm>>.Failure("500", ex.Message);
        }
    }
    
    
    public Task<Dictionary<string, bool>> GetOnlineStatusesAsync(List<string> userIds) =>
        Task.FromResult(new Dictionary<string, bool>());
}