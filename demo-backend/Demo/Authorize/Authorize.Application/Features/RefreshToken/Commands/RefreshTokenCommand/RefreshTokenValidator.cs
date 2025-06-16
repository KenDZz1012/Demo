using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorize.Application.Features.RefreshToken.Commands.RefreshTokenCommand
{
    public class RefreshTokenValidator : AbstractValidator<RefreshToken>
    {
        public RefreshTokenValidator()
        {
            RuleFor(x => x.userID)
                   .NotEmpty().NotNull()
                   .WithMessage("userID must not empty!");

            RuleFor(x => x.refreshToken)
                .NotEmpty().NotNull()
                .WithMessage("refreshToken must not empty!");
        }
    }
}
