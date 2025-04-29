using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Account.Application.Contracts.Persistence;
using Account.Application.Features.User.Queries.GetUserQuery;
using AutoMapper;
using MediatR;
using Service.Lib.BaseResponse;

namespace Account.Application.Features.User.Queries.GetUserByIDQuery
{
    public class GetUserByIDHandler : IRequestHandler<GetUserByID, ApiResponse<GetUserByIDVm>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUserByIDHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<ApiResponse<GetUserByIDVm>> Handle(GetUserByID request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(request.ID);
                return user != null ? ApiResponse<GetUserByIDVm>.Success(_mapper.Map<GetUserByIDVm>(user), "Lấy user thành công") : ApiResponse<GetUserByIDVm>.Failure("404", "User not found");
            }
            catch (Exception ex)
            {
                return await Task.FromResult(ApiResponse<GetUserByIDVm>.Failure("500", ex.Message));
            }
        }
    }
}
