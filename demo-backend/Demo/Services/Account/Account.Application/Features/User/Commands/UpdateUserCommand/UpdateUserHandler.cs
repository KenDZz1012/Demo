using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Account.Application.Contracts.Persistence;
using Account.Application.Features.User.Queries.GetUsersQuery;
using AutoMapper;
using MediatR;
using Service.Lib.BaseResponse;

namespace Account.Application.Features.User.Commands.UpdateUserCommand
{
    public class UpdateUserHandler : IRequestHandler<UpdateUser, ApiResponse<Guid>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UpdateUserHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<ApiResponse<Guid>> Handle(UpdateUser request, CancellationToken cancellationToken)
        {
            try
            {
                var existingUser = await _userRepository.GetByIdAsync(request.ID);
                if (existingUser == null)
                {
                    return ApiResponse<Guid>.Failure("404", "User not found");
                }
                var existingUserName = await _userRepository.GetAllAsync(new GetUsers(request.UserName, null, null));
                var existingEmail = await _userRepository.GetAllAsync(new GetUsers(null, request.Email, null));
                if (existingUserName.Count > 0 && !existingUserName.Any(x => x.Id == request.ID))
                {
                    return ApiResponse<Guid>.Failure("400", "UserName already exists");
                }
                else if (existingEmail.Count > 0 && !existingUserName.Any(x => x.Id == request.ID))
                {
                    return ApiResponse<Guid>.Failure("400", "Email already exists");
                }
                else
                {
                    var user = _mapper.Map<Account.Domain.Entities.User>(request);
                    var isUpdatedSuccess = await _userRepository.UpdateAsync(user);
                    return isUpdatedSuccess ? ApiResponse<Guid>.Success(user.Id, "Cập nhật user thành công") : ApiResponse<Guid>.Failure("500", "Không cập nhật được user");
                }
            }
            catch (Exception ex)
            {
                return await Task.FromResult(ApiResponse<Guid>.Failure("500", ex.Message));
            }
        }
    }
}
