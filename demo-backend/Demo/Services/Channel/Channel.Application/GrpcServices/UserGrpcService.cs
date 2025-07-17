using Account.Grpc.Protos;

namespace Channel.Application.GrpcServices
{
    public class UserGrpcService
    {
        private readonly AccountProtoSerivce.AccountProtoSerivceClient _accountGrpcClient;

        public UserGrpcService(AccountProtoSerivce.AccountProtoSerivceClient accountGrpcClient)
        {
            _accountGrpcClient = accountGrpcClient;
        }

        public async Task<GetUsersInfoInChannelResponse> GetUserInfoInChannel(List<string> userId)
        {
            var request = new GetUserInfoInChannelRequest();
            request.UserId.AddRange(userId);
            return await _accountGrpcClient.GetUserInfoInChannelAsync(request);
        }
    }
}
