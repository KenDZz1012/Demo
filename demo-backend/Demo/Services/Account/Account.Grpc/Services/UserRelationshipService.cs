using Account.Application.Contracts.Persistence;
using Account.Grpc.Protos;
using AutoMapper;
using Grpc.Core;

namespace Account.Grpc.Services;

public class UserRelationshipService: AccountProtoSerivce.AccountProtoSerivceBase
{
    private readonly IUserRelationshipRepository _userRelationshipService;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    public UserRelationshipService(IUserRelationshipRepository userRelationshipService, IMapper mapper, IUserRepository userRepository)
    {
        _userRelationshipService = userRelationshipService;
        _mapper = mapper;
        _userRepository = userRepository;
    }
    
    public override async Task<GetListFriendResponse> GetListFriend(GetListFriendRequest request, ServerCallContext context)
    {
        var response = new GetListFriendResponse();
        var user = await _userRepository.CheckExistUserName(request.UserId);
        if(user == null) return null;
        var result = await _userRelationshipService.GetUserRelationships(user.Id);
        if (result.Any())
        {
            response.Friends.AddRange(result.Select(userR => _mapper.Map<UserRelationshipModel>(userR)));
        }
        return response;
    }
}