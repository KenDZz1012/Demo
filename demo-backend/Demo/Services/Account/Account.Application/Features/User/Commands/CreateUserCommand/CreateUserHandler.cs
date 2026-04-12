using Account.Application.Contracts.Persistence;
using Account.Application.Models.Emails;
using Account.Domain.Entities;
using AutoMapper;
using MediatR;
using Service.Lib.BaseResponse;
using Servivce.HttpHelper.Dtos.Authorize;
using Servivce.HttpHelper.Services;

namespace Account.Application.Features.User.Commands.CreateUserCommand;

public class CreateUserHandler : IRequestHandler<CreateUser, ApiResponse<Guid>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;
    private readonly AuthorizeHttpService _authorizeHttp;

    public CreateUserHandler(
        IUserRepository userRepository,
        IMapper mapper,
        IEmailService emailService,
        AuthorizeHttpService authorizeHttp)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _emailService = emailService;
        _authorizeHttp = authorizeHttp;
    }

    public async Task<ApiResponse<Guid>> Handle(CreateUser request, CancellationToken cancellationToken)
    {
        try
        {
            if (await _userRepository.CheckExistUserName(request.UserName!) != null)
                return ApiResponse<Guid>.Failure("400", "Tên người dùng đã tồn tại.");

            if (await _userRepository.CheckExistEmail(request.Email!) != null)
                return ApiResponse<Guid>.Failure("400", "Email đã tồn tại.");

            var user = _mapper.Map<Domain.Entities.User>(request);
            user.Id = Guid.NewGuid();

            if (!await _userRepository.AddAsync(user))
                return ApiResponse<Guid>.Failure("500", "Không thêm được user");

            if (string.IsNullOrWhiteSpace(request.Password))
                return ApiResponse<Guid>.Success(user.Id, "Thêm user thành công");

            var identityResponse = await _authorizeHttp.CreateIdentityUserAsync(
                MapToIdentityDto(user, request.Password),
                cancellationToken);

            if (identityResponse is not { IsSuccess: true })
            {
                await _userRepository.DeleteAsync(user);
                var message = identityResponse?.Message ?? "Không tạo được tài khoản đăng nhập (Authorize).";
                var code = string.IsNullOrEmpty(identityResponse?.ErrorCode) ? "502" : identityResponse.ErrorCode;
                return ApiResponse<Guid>.Failure(code, message);
            }

            return ApiResponse<Guid>.Success(user.Id, "Thêm user thành công");
        }
        catch (Exception ex)
        {
            return ApiResponse<Guid>.Failure("500", ex.Message);
        }
    }

    private static CreateIdentityUserHttpDto MapToIdentityDto(Domain.Entities.User user, string password) => new()
    {
        UserId = user.Id,
        UserName = user.UserName,
        Email = user.Email,
        AvatarUrl = user.AvatarUrl,
        Password = password,
        DisplayName = user.DisplayName,
        AccountStatus = "active"
    };
}
