using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Channel.Application.Features.Server.Commands.UpdateServerIcon
{
    public class UpdateServerIconValidator : AbstractValidator<UpdateServerIcon>
    {
        public UpdateServerIconValidator() 
        {
            RuleFor(x => x.IconUrl)
                .NotNull().WithMessage("IconUrl is required.")
                .Must(file => file.Length > 0).WithMessage("IconUrl must not be empty.")
                .Must(file => file.Length <= 5 * 1024 * 1024).WithMessage("IconUrl must not exceed 5MB.")
                .Must(file => file.ContentType == "image/png" || file.ContentType == "image/jpeg")
                .WithMessage("IconUrl must be a PNG or JPEG image.");
        }
    }
}
