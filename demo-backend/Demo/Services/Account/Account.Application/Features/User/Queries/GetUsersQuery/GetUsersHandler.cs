using Account.Application.Contracts.Persistence;
using AutoMapper;
using MediatR;
using Service.Lib.BaseResponse;

namespace Account.Application.Features.User.Queries.GetUsersQuery
{
    public class GetUsersHandler : IRequestHandler<GetUsers, ApiResponse<List<GetUsersVm>>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUsersHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<ApiResponse<List<GetUsersVm>>> Handle(GetUsers request, CancellationToken cancellationToken)
        {
            try
            {
                var users = await _userRepository.GetAllAsync(request);
                return ApiResponse<List<GetUsersVm>>.Success(_mapper.Map<List<GetUsersVm>>(users), "Lấy danh sách user thành công");
            }
            catch(Exception ex)
            {
                return ApiResponse<List<GetUsersVm>>.Failure("500", ex.Message);
            }
        }
    }
}
