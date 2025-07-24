using System.Net.Http.Json;
using Account.Application.Contracts.Persistence;
using AutoMapper;
using MediatR;
using Service.Lib.BaseResponse;
using Microsoft.Extensions.DependencyInjection;

namespace Account.Application.Features.UserRelationship.Queries.GetListFriendQuery;

public class GetListFriendHandler : IRequestHandler<GetListFriend, ApiResponse<List<GetListFriendVm>>>
{
    private readonly IUserRelationshipRepository _userRelationshipRepository;
    private readonly IMapper _mapper;
    private readonly HttpClient _httpClient;

    public GetListFriendHandler(IUserRelationshipRepository userRelationshipRepository, IMapper mapper, IHttpClientFactory httpClientFactory)
    {
        _userRelationshipRepository = userRelationshipRepository;
        _mapper = mapper;
        _httpClient = httpClientFactory.CreateClient("PresenceService");
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

            var friendIds = friends.Select(f => f.Id).ToList();
            var onlineStatuses = await GetOnlineStatusesAsync(friendIds);

            foreach (var friend in friends)
            {
                friend.IsOnline = onlineStatuses.TryGetValue(friend.Id, out var isOnline) && isOnline;
            }

            return ApiResponse<List<GetListFriendVm>>.Success(friends);
        }
        catch (Exception ex)
        {
            return ApiResponse<List<GetListFriendVm>>.Failure("500", ex.Message);
        }
    }
    
    
    public async Task<Dictionary<Guid, bool>> GetOnlineStatusesAsync(List<Guid> userIds)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("v1/presence/batch-status",  userIds );
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<Dictionary<Guid, bool>>();
            return result ?? new Dictionary<Guid, bool>();
        }
        catch
        {
            // Log error nếu cần
            return new Dictionary<Guid, bool>();
        }
    }


}