using Account.Grpc.Protos;

namespace Presence.GrpcService;

public class UserGrpcService
{
    private readonly AccountProtoSerivce.AccountProtoSerivceClient _accountGrpcClient;

    public UserGrpcService(AccountProtoSerivce.AccountProtoSerivceClient accountGrpcClient)
    {
        _accountGrpcClient = accountGrpcClient;
    }

    public async Task<GetListFriendResponse> GetListFriend(GetListFriendRequest request)
    {
        var friends = await _accountGrpcClient.GetListFriendAsync(request);
        return friends;
    }
}