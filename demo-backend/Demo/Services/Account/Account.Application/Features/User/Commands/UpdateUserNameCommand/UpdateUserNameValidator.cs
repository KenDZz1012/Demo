using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Account.Application.Features.User.Commands.UpdateUserCommand;
using FluentValidation;

namespace Account.Application.Features.User.Commands.UpdateUserNameCommand
{
    public class UpdateUserNameValidator : AbstractValidator<UpdateUserName>
    {
        public UpdateUserNameValidator()
        {
            RuleFor(x => x.ID)
                .NotEmpty().NotNull()
                .WithMessage("ID không được trống.");
            RuleFor(x => x.UserName)
                .NotEmpty().NotNull()
                .WithMessage("Tên đăng nhập không được trống.")
                .Length(0, 50)
                .WithMessage("Tên đăng nhập không được quá 50 ký tự.");
            RuleFor(x => x.PasswordHash)
                .NotEmpty().NotNull()
                .WithMessage("Mật khẩu không được trống.");
        }
    }
}
