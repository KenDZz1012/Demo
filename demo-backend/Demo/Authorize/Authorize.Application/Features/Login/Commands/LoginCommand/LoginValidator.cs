using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorize.Application.Features.Login.Commands.LoginCommand
{
    public class LoginValidator : AbstractValidator<Login>
    {
        public LoginValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().NotNull()
                .WithMessage("Username must not empty!");

            RuleFor(x => x.Password)
                .NotEmpty().NotNull()
                .WithMessage("Mật khẩu không được trống.");
        }
    }
}
