using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorize.Application.Features.Logout.Commands.LogoutCommand
{
    public class LogoutValidator : AbstractValidator<Logout>
    {
        public LogoutValidator()
        {
            RuleFor(x => x.RefreshToken)
              .NotEmpty().NotNull()
              .WithMessage("refreshToken must not empty!");
        }
    }
}
