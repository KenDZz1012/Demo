using FluentValidation;

namespace Account.Application.Features.User.Commands.CreateUserCommand;

public class CreateUserValidator : AbstractValidator<CreateUser>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.DisplayName)
            .NotEmpty()
            .WithMessage("Tên hiển thị không được trống.")
            .NotNull()
            .WithMessage("Tên hiển thị không được trống.")
            .Length(0, 250)
            .WithMessage("Tên hiển thị không được quá 250 ký tự.");

        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithMessage("Tên người dùng không được trống.")
            .NotNull()
            .WithMessage("Tên người dùng không được trống.")
            .Length(0, 50)
            .WithMessage("Tên người dùng không được quá 50 ký tự.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email không được trống.")
            .NotNull()
            .WithMessage("Email không được trống.")
            .EmailAddress()
            .WithMessage("Email không hợp lệ.")
            .Length(0, 100)
            .WithMessage("Email không được quá 100 ký tự");

        RuleFor(x => x.DateOfBirth)
            .NotNull()
            .WithMessage("Ngày sinh không được trống")
            .LessThan(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("Ngày sinh phải nhỏ hơn ngày hiện tại");

        RuleFor(x => x.Password)
            .MinimumLength(6)
            .When(x => !string.IsNullOrWhiteSpace(x.Password))
            .WithMessage("Mật khẩu tối thiểu 6 ký tự.");
    }
}
