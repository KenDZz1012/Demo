using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Account.Application.Features.UserRelationship.Commands.CreateUserRelationshipCommand
{
    public class CreateUserRelationshipValidator : AbstractValidator<CreateUserRelationship>
    {
        public CreateUserRelationshipValidator()
        {
            RuleFor(x => x.RequesterId)
                .NotEmpty().NotNull()
                .WithMessage("RequesterId không được trống.");
            RuleFor(x => x.AddresseeName)
                .NotEmpty().NotNull()
                .WithMessage("Addressee Name không được trống.");
        }
    }
}
