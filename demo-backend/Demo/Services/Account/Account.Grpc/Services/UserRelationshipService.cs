using Account.Application.Contracts.Persistence;
using Account.Grpc.Protos;
using AutoMapper;
using Grpc.Core;

namespace Account.Grpc.Services;

public class UserRelationshipService : UserRelationshipProtoSerivce.UserRelationshipProtoSerivceBase
{
    private readonly IUserRelationshipRepository _userRelationshipService;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public UserRelationshipService(IUserRelationshipRepository userRelationshipService, IMapper mapper,
        IUserRepository userRepository)
    {
        _userRelationshipService = userRelationshipService;
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public override async Task<GetListFriendResponse> GetListFriend(GetListFriendRequest request, ServerCallContext context)
    {
        var user = await _userRepository.CheckExistUserName(request.UserId);
        if (user == null)
        {
            Console.WriteLine($"❌ User not found: {request.UserId}");
            throw new RpcException(new Status(StatusCode.NotFound, "User not found"));
        }

        var relationships = await _userRelationshipService.GetUserRelationships(user.Id);

        var friends = relationships.Select(item => new UserRelationshipModel
        {
            Id = (item.AddresseeId == user.Id ? item.RequesterId : item.AddresseeId).ToString(),
            UserName = item.AddresseeId == user.Id ? item.Requester.UserName : item.Addressee.UserName
        });

        var response = new GetListFriendResponse();
        response.Friends.AddRange(friends);

        Console.WriteLine($"✅ Found {response.Friends.Count} friends for user {user.UserName}");

        return response;
    }

}