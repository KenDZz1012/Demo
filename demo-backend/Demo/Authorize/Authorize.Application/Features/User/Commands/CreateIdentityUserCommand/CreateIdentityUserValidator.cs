using FluentValidation;

namespace Authorize.Application.Features.User.Commands.CreateIdentityUserCommand;

public sealed class CreateIdentityUserValidator : AbstractValidator<CreateIdentityUser>
{
    public CreateIdentityUserValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithMessage("Tên đăng nhập không được trống.")
            .MaximumLength(256)
            .WithMessage("Tên đăng nhập không được quá 256 ký tự.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email không được trống.")
            .EmailAddress()
            .WithMessage("Email không hợp lệ.")
            .MaximumLength(256);

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Mật khẩu không được trống.")
            .MinimumLength(6)
            .WithMessage("Mật khẩu tối thiểu 6 ký tự.");

        RuleFor(x => x.DisplayName)
            .MaximumLength(256)
            .When(x => !string.IsNullOrEmpty(x.DisplayName));

        RuleFor(x => x.AvatarUrl)
            .MaximumLength(2048)
            .When(x => !string.IsNullOrEmpty(x.AvatarUrl));

        RuleFor(x => x.AccountStatus)
            .Must(s => s is "active" or "banned" or "pending")
            .WithMessage("AccountStatus phải là active, banned hoặc pending.");
    }
}
