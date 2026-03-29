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
using Account.Domain.Entities;
using Service.Lib.Password;
using Service.Lib.Keycloak;
using Account.Application.Models.Emails;
using Microsoft.EntityFrameworkCore;

namespace Account.Application.Features.User.Commands.CreateUserCommand
{
    public class CreateUserHandler : IRequestHandler<CreateUser, ApiResponse<Guid>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public CreateUserHandler(IUserRepository userRepository, IMapper mapper, IEmailService emailService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _emailService = emailService;
        }
        public async Task<ApiResponse<Guid>> Handle(CreateUser request, CancellationToken cancellationToken)
        {
            try
            {
                var existingUserName = await _userRepository.CheckExistUserName(request.UserName);
                var existingEmail = await _userRepository.CheckExistEmail(request.Email);
                if (existingUserName != null)
                {
                    return ApiResponse<Guid>.Failure("400", "Tên người dùng đã tồn tại.");
                }
                else if (existingEmail != null)
                {
                    return ApiResponse<Guid>.Failure("400", "Email đã tồn tại.");
                }
                else
                {
                    var user = _mapper.Map<Account.Domain.Entities.User>(request);
                    var isCreatedSuccess = await _userRepository.AddAsync(user);
                    return isCreatedSuccess ? ApiResponse<Guid>.Success(user.Id, "Thêm user thành công") : ApiResponse<Guid>.Failure("500", "Không thêm được user");
                }
            }
            catch (Exception ex)
            {
                return await Task.FromResult(ApiResponse<Guid>.Failure("500", ex.Message));
            }
        }

        public async Task SendMail(string emailTo)
        {
            var email = new Email()
            {
                To = emailTo,
                Body = $"Chào bạn,\n\nTài khoản của bạn đã được tạo thành công. Vui lòng đăng nhập để sử dụng dịch vụ.\n\nTrân trọng,\nĐội ngũ hỗ trợ.",
                Subject = "Thông báo tạo tài khoản thành công"
            };
            try
            {
                await _emailService.SendMail(email);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }
    }
}
