using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Account.Application.Features.User.Commands.UpdateEmailCommand
{
    public class UpdateEmailValidator : AbstractValidator<UpdateEmail>
    {
        public UpdateEmailValidator()
        {
            RuleFor(x => x.ID)
                .NotEmpty().NotNull()
                .WithMessage("ID không được trống");

            RuleFor(x => x.Email)
                .NotEmpty().NotNull()
                .WithMessage("Email không được trống.")
                .Length(0, 100)
                .WithMessage("UserName không được quá 100 ký tự.");

            RuleFor(x => x.PasswordHash)
                .NotEmpty().NotNull()
                .WithMessage("Mật khẩu không được trống");
        }
    }
}
