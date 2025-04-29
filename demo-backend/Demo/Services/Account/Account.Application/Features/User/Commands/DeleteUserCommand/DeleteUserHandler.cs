using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Account.Application.Contracts.Persistence;
using Account.Domain.Entities;
using AutoMapper;
using Azure.Core;
using MediatR;
using Service.Lib.BaseResponse;

namespace Account.Application.Features.User.Commands.DeleteUserCommand
{
    public class DeleteUserHandler : IRequestHandler<DeleteUser, ApiResponse<Guid>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public DeleteUserHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<ApiResponse<Guid>> Handle(DeleteUser request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(request.ID);
                if (user == null)
                {
                    return ApiResponse<Guid>.Failure("404", "User not found");
                }
                var success = await _userRepository.DeleteAsync(user);
                return success ? ApiResponse<Guid>.Success(request.ID, "User deleted successfully") : ApiResponse<Guid>.Failure("500", "Xóa user không thành công");
            }
            catch (Exception ex)
            {
                return await Task.FromResult(ApiResponse<Guid>.Failure("500", ex.Message));
            }
        }
    }
}
