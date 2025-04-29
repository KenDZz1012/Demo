using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Account.Application.Features.User.Commands.CreateUserCommand;
using FluentValidation;

namespace Account.Application.Features.User.Commands.DeleteUserCommand
{
    public class DeleteUserValidator : AbstractValidator<DeleteUser>
    {
        public DeleteUserValidator()
        {
            RuleFor(x => x.ID)
                .NotEmpty().NotNull()
                .WithMessage("{ID} is required.");
        }
    }
}
