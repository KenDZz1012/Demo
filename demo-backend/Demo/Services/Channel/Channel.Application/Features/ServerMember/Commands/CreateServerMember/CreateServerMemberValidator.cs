using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Channel.Application.Features.ServerMember.Commands.CreateServerMember
{
    public class CreateServerMemberValidator : AbstractValidator<CreateServerMember>
    {
        public CreateServerMemberValidator()
        {
            RuleFor(x => x.ServerId)
                .NotEmpty().NotNull()
                .WithMessage("Tên server không được trống");

            RuleFor(x => x.UserId)
                .NotEmpty().NotNull()
                .WithMessage("User không được trống");
            
            RuleFor(x => x.Role)
                .NotEmpty().NotNull()
                .WithMessage("Role không được trống");
        }
    }
}
