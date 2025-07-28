using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Channel.Application.Features.Server.Commands.DeleteServer
{
    public class DeleteServerValidator: AbstractValidator<DeleteServer>
    {
        public DeleteServerValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().NotNull()
                .WithMessage("Id must not empty");
        }
    }
}
