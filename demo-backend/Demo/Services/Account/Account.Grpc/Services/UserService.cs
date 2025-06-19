using Grpc.Core;
using Account.Grpc.Protos;
using System.Threading.Tasks;
using AutoMapper;
using Account.Application.Contracts.Persistence;

namespace Account.Grpc.Services
{
    public class UserService : AccountProtoSerivce.AccountProtoSerivceBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }


        public override async Task<UserModel> GetUserByUserNameOrEmail(GetUserByUserNameOrEmailRequest request, ServerCallContext context)
        {
            var user = await _userRepository.GetUserByUserNameOrEmail(request.Search);
            if(user == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "User not found"));
            }
            var userModel = _mapper.Map<UserModel>(user);
            return userModel;
        }
    }
}
