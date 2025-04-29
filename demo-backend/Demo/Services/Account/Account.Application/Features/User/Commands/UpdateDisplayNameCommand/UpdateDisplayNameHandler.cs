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

namespace Account.Application.Features.User.Commands.UpdateDisplayNameCommand
{
    public class UpdateDisplayNameHandler : IRequestHandler<UpdateDisplayName, ApiResponse<Guid>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UpdateDisplayNameHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<ApiResponse<Guid>> Handle(UpdateDisplayName request, CancellationToken cancellationToken)
        {
            try
            {
                var userUpdate = await _userRepository.GetByIdAsync(request.ID);
                if (userUpdate != null)
                {
                    if (await PasswordMD5.CreateMD5(request.PasswordHash) != userUpdate.PasswordHash)
                    {
                        return ApiResponse<Guid>.Failure("500", "Mật khẩu không đúng");
                    }
                    else
                    {
                        userUpdate.DisplayName = request.DisplayName;
                        var isUpdatedSuccess = await _userRepository.UpdateAsync(userUpdate);
                        return isUpdatedSuccess ? ApiResponse<Guid>.Success(userUpdate.ID, "Cập nhật Email thành công") : ApiResponse<Guid>.Failure("500", "Không cập nhật được Email");
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
