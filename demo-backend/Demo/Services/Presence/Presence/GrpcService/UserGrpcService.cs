using Account.Grpc.Protos;

namespace Presence.GrpcService;

public class UserGrpcService
{
    private readonly UserRelationshipProtoSerivce.UserRelationshipProtoSerivceClient _accountGrpcClient;

    public UserGrpcService(UserRelationshipProtoSerivce.UserRelationshipProtoSerivceClient accountGrpcClient)
    {
        _accountGrpcClient = accountGrpcClient;
    }

    public async Task<GetListFriendResponse> GetListFriend(GetListFriendRequest request)
    {
        var friends = await _accountGrpcClient.GetListFriendAsync(request);
        return friends;
    }
}