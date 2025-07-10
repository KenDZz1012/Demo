using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Channel.Application.Features.Channel.Commands.CreateChannel
{
    public class CreateChannelValidator: AbstractValidator<CreateChannel>
    {
        public CreateChannelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().NotNull()
                .WithMessage("Tên server không được trống");

            RuleFor(x => x.ServerId)
                .NotEmpty().NotNull()
                .WithMessage("Server không được trống");
            
            RuleFor(x => x.Type)
                .NotEmpty().NotNull()
                .WithMessage("Type không được trống");
        }
    }
}
