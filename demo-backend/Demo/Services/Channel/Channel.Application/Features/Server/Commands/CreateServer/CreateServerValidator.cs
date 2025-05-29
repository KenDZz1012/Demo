using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Channel.Application.Features.Server.Commands.CreateServer
{
    public class CreateServerValidator : AbstractValidator<CreateServer>
    {
        public CreateServerValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().NotNull()
                .WithMessage("Tên server không được trống");

            RuleFor(x => x.OwnerId)
                .NotEmpty().NotNull()
                .WithMessage("Người tạo không được trống");
        }
    }
}
