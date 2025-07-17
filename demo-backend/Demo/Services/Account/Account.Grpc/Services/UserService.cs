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

        public override async Task<GetUsersInfoInChannelResponse> GetUserInfoInChannel(GetUserInfoInChannelRequest request,
            ServerCallContext context)
        {
            var userGuids = request.UserId
                .Select(id => Guid.TryParse(id, out var guid) ? guid : Guid.Empty)
                .Where(guid => guid != Guid.Empty)
                .ToList();
            var users = await _userRepository.GetUserByIds(userGuids);
           
            if (users == null || !users.Any())
            {
                throw new RpcException(new Status(StatusCode.NotFound, "No users found"));
            }
            var response = new GetUsersInfoInChannelResponse();
            response.Users.AddRange(users.Select(user => _mapper.Map<UserModel>(user)));
            return response;
        }
    }
}
