using Account.Application.Contracts.Persistence;
using Account.Grpc.Protos;
using AutoMapper;
using Grpc.Core;

namespace Account.Grpc.Services;

public class UserRelationshipService: AccountProtoSerivce.AccountProtoSerivceBase
{
    private readonly IUserRelationshipRepository _userRelationshipService;
    private readonly IMapper _mapper;
    
    public UserRelationshipService(IUserRelationshipRepository userRelationshipService, IMapper mapper)
    {
        _userRelationshipService = userRelationshipService;
        _mapper = mapper;
    }
    
    public override async Task<GetListFriendResponse> GetListFriend(GetListFriendRequest request, ServerCallContext context)
    {
        var response = new GetListFriendResponse();
        var result = await _userRelationshipService.GetUserRelationships(Guid.TryParse(request.UserId, out var guid) ? guid : Guid.Empty);
        if (result.Any())
        {
            response.Friends.AddRange(result.Select(userR => _mapper.Map<UserRelationshipModel>(userR)));
        }
        return response;
    }
}