using MediatR;
using Service.Lib.BaseResponse;

namespace Account.Application.Features.User.Commands.CreateUserCommand;

public class CreateUser : IRequest<ApiResponse<Guid>>
{
    public string? UserName { get; set; }

    public string? DisplayName { get; set; }

    public string? Email { get; set; }

    public DateOnly DateOfBirth { get; set; }
    
    public string? Password { get; set; }
}
