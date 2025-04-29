using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Account.Application.Features.User.Commands.UpdateUserCommand
{
    public class UpdateUserValidator : AbstractValidator<UpdateUser>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.ID)
                .NotEmpty().NotNull()
                .WithMessage("{ID} is required.");
            RuleFor(x => x.UserName)
                .NotEmpty().NotNull()
                .WithMessage("{UserName} is required.")
                .Length(50)
                .WithMessage("UserName không được quá 50 ký tự.");
            RuleFor(x => x.PasswordHash)
                .NotEmpty().NotNull()
                .WithMessage("{PasswordHash} is required.");
            RuleFor(x => x.Email)
                .NotEmpty().NotNull()
                .WithMessage("{Email} is required.")
                .EmailAddress()
                .WithMessage("Email không hợp lệ.")
                .Length(100)
                .WithMessage("Email không được quá 100 ký tự");
            RuleFor(x => x.Status)
                .NotEmpty().NotNull()
                .WithMessage("{Status} is required.")
                .Length(20)
                .WithMessage("Status không được quá 20 ký tự.");
        }
    }
}
