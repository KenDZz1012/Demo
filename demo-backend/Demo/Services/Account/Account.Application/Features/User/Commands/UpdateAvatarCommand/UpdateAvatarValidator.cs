using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Account.Application.Features.User.Commands.UpdateAvatarCommand
{
    public class UpdateAvatarValidator : AbstractValidator<UpdateAvatar>
    {
        public UpdateAvatarValidator()
        {
            RuleFor(x => x.ID)
                .NotEmpty().NotNull()
                .WithMessage("ID không được trống");
        }
    }
}
