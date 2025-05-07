using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Account.Application.Features.User.Commands.UpdatePasswordCommand
{
    public class UpdatePasswordValidator : AbstractValidator<UpdatePassword>
    {
        public UpdatePasswordValidator()
        {
            RuleFor(x => x.ID)
                 .NotEmpty().NotNull()
                 .WithMessage("ID không được trống");

            RuleFor(x => x.NewPasswordHash)
                .NotEmpty().NotNull()
                .WithMessage("Password mới không được trống.");

            RuleFor(x => x.PasswordHash)
                .NotEmpty().NotNull()
                .WithMessage("Mật khẩu không được trống");
        }
    }
}
