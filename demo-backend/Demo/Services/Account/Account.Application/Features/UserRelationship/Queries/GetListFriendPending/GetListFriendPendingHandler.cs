using Account.Application.Contracts.Persistence;
using AutoMapper;
using MediatR;
using Service.Lib.BaseResponse;

namespace Account.Application.Features.UserRelationship.Queries.GetListFriendPending;

public class GetListFriendPendingHandler : IRequestHandler<GetListFriendPending, ApiResponse<List<GetListFriendPendingVm>>>
{
    private readonly IUserRelationshipRepository _userRelationshipRepository;
    private readonly IMapper _mapper;
    private readonly HttpClient _httpClient;
    
    public GetListFriendPendingHandler(IUserRelationshipRepository userRelationshipRepository, IMapper mapper, IHttpClientFactory httpClientFactory)
    {
        _userRelationshipRepository = userRelationshipRepository;
        _mapper = mapper;
        _httpClient = httpClientFactory.CreateClient("PresenceService");
    }
    
    public async Task<ApiResponse<List<GetListFriendPendingVm>>> Handle(GetListFriendPending request, CancellationToken cancellationToken)
    {
        try
        {
            var userRelationships = await _userRelationshipRepository.GetUserRelationships(request.UserId);

            var friends = userRelationships.Select(x =>
            {
                var friend = x.RequesterId == request.UserId ? x.Addressee : x.Requester;
                return _mapper.Map<GetListFriendPendingVm>(friend);
            }).ToList();

            var friendIds = friends.Select(f => f.UserName).ToList();
            var onlineStatuses = await GetOnlineStatusesAsync(friendIds);

            foreach (var friend in friends)
            {
                friend.IsOnline = onlineStatuses.TryGetValue(friend.UserName, out var isOnline) && isOnline;
            }

            return ApiResponse<List<GetListFriendPendingVm>>.Success(friends);
        }
        catch (Exception ex)
        {
            return ApiResponse<List<GetListFriendPendingVm>>.Failure("500", ex.Message);
        }
        
    }
}