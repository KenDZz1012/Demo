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

namespace Account.Application.Features.User.Commands.CreateUserCommand
{
    public class CreateUserHandler : IRequestHandler<CreateUser, ApiResponse<Guid>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IKeycloakService _keycloakService;

        public CreateUserHandler(IUserRepository userRepository, IMapper mapper, IKeycloakService keycloakService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _keycloakService = keycloakService;
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
                    string originPassword = request.PasswordHash;
                    request.PasswordHash = await PasswordMD5.CreateMD5(request.PasswordHash);
                    var user = _mapper.Map<Account.Domain.Entities.User>(request);
                    var isCreatedSuccess = await _userRepository.AddAsync(user);
                    if (isCreatedSuccess)
                    {
                        var newUserKeycloak = new KeycloakUserDto()
                        {
                            UserName = user.UserName,
                            Email = user.Email,
                            FirstName = user.DisplayName,
                            LastName = user.DisplayName,
                            Password = originPassword
                        };
                        await _keycloakService.CreateUserAsync(newUserKeycloak);
                        if (request.IsAdmin.HasValue)
                        {
                            if (request.IsAdmin.Value)
                            {
                                await _keycloakService.AssignRoleAsync(user.UserName, "Admin");
                            }
                            else
                            {
                                await _keycloakService.AssignRoleAsync(user.UserName, "Client");
                            }
                        }
                    }
                    return isCreatedSuccess ? ApiResponse<Guid>.Success(user.Id, "Thêm user thành công") : ApiResponse<Guid>.Failure("500", "Không thêm được user");
                }
            }
            catch (Exception ex)
            {
                return await Task.FromResult(ApiResponse<Guid>.Failure("500", ex.Message));
            }
        }
    }
}
