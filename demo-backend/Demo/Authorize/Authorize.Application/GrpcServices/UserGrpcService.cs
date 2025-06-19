using Account.Grpc.Protos;

namespace Authorize.GrpcServices
{
    public class UserGrpcService
    {
        private readonly AccountProtoSerivce.AccountProtoSerivceClient _accountGrpcClient;

        public UserGrpcService(AccountProtoSerivce.AccountProtoSerivceClient accountGrpcClient)
        {
            _accountGrpcClient = accountGrpcClient;
        }

        public async Task<UserModel> GetUserByUserNameOrEmailAsync(string search)
        {
            var request = new GetUserByUserNameOrEmailRequest { Search = search };
            return await _accountGrpcClient.GetUserByUserNameOrEmailAsync(request);
        }
    }
}
