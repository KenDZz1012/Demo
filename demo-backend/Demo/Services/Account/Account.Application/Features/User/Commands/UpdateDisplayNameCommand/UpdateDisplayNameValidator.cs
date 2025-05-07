using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Account.Application.Features.User.Commands.UpdateDisplayNameCommand
{
    public class UpdateDisplayNameValidator : AbstractValidator<UpdateDisplayName>
    {
        public UpdateDisplayNameValidator()
        {
            RuleFor(x => x.ID)
                .NotEmpty().NotNull()
                .WithMessage("ID không được trống");

            RuleFor(x => x.DisplayName)
                .NotEmpty().NotNull()
                .WithMessage("Tên hiển thị không được trống.")
                .Length(0, 250)
                .WithMessage("Tên hiển thị không được quá 250 ký tự.");

            RuleFor(x => x.PasswordHash)
                .NotEmpty().NotNull()
                .WithMessage("Mật khẩu không được trống");
        }
    }
}
