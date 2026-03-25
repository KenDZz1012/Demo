using FluentValidation;

namespace Guild.Application.Features.Guild.Commands.CreateGuild
{
    public class CreateGuildValidator : AbstractValidator<CreateGuild>
    {
        public CreateGuildValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Guild name is required")
                .MaximumLength(100).WithMessage("Guild name must not exceed 100 characters");
            RuleFor(x => x.OwnerId).NotEmpty().WithMessage("OwnerId is required");
        }
    }
}
