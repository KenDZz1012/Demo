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

        public async Task<UserModel> GetUserInfoInChannel(string userId)
        {
            var request = new GetUserInfoInChannelRequest() { UserId = userId };
            return await _accountGrpcClient.GetUserInfoInChannelAsync(request);
        }
    }
}
