using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Account.Application.Contracts.Persistence;
using AutoMapper;
using MediatR;
using Service.Lib.BaseResponse;
using Service.Lib.Password;

namespace Account.Application.Features.User.Commands.UpdateUserNameCommand
{
    public class UpdateUserNameHandler : IRequestHandler<UpdateUserName, ApiResponse<Guid>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UpdateUserNameHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<ApiResponse<Guid>> Handle(UpdateUserName request, CancellationToken cancellationToken)
        {
            try
            {
                var existingUserName = await _userRepository.CheckExistUserName(request.UserName);
                if (existingUserName != null && existingUserName.Id != request.ID)
                {
                    return ApiResponse<Guid>.Failure("500", "Tên đăng nhập đã tồn tại");
                }
                var userUpdate = await _userRepository.GetByIdAsync(request.ID);
                if (userUpdate != null)
                {
                    if (await PasswordMD5.CreateMD5(request.PasswordHash) != userUpdate.PasswordHash)
                    {
                        return ApiResponse<Guid>.Failure("500", "Mật khẩu không đúng");
                    }
                    else
                    {
                        userUpdate.UserName = request.UserName;
                        var isUpdatedSuccess = await _userRepository.UpdateAsync(userUpdate);
                        return isUpdatedSuccess ? ApiResponse<Guid>.Success(userUpdate.Id, "Cập nhật tên đăng nhập thành công") : ApiResponse<Guid>.Failure("500", "Không cập nhật được tên đăng nhập");
                    }
                }
                else
                {
                    return ApiResponse<Guid>.Failure("404", "User không tồn tại");
                }
            }
            catch (Exception ex)
            {
                return await Task.FromResult(ApiResponse<Guid>.Failure("500", ex.Message));
            }
        }
    }
}
