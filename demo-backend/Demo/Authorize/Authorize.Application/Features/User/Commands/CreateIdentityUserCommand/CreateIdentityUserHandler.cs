using Authorize.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Service.Lib.BaseResponse;

namespace Authorize.Application.Features.User.Commands.CreateIdentityUserCommand;

public sealed class CreateIdentityUserHandler : IRequestHandler<CreateIdentityUser, ApiResponse<Guid>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<CreateIdentityUserHandler> _logger;

    public CreateIdentityUserHandler(UserManager<ApplicationUser> userManager,  ILogger<CreateIdentityUserHandler> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<ApiResponse<Guid>> Handle(CreateIdentityUser request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Handling CreateIdentityUser");
            var id = request.UserId ?? Guid.NewGuid();

            var user = new ApplicationUser
            {
                Id = id,
                UserName = request.UserName,
                Email = request.Email,
                DisplayName = request.DisplayName ?? request.UserName,
                AvatarUrl = request.AvatarUrl ?? string.Empty,
                AccountStatus = request.AccountStatus,
                EmailConfirmed = false
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(" ", result.Errors.Select(e => e.Description));
                return ApiResponse<Guid>.Failure("400", errors);
            }
            return ApiResponse<Guid>.Success(user.Id, "Tạo user Identity thành công.");
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex.Message);
            return ApiResponse<Guid>.Failure(ex.Message, ex.Message);
        }
    }
}
