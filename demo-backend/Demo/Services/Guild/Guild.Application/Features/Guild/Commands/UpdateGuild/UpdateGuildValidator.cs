using FluentValidation;

namespace Guild.Application.Features.Guild.Commands.UpdateGuild
{
    public class UpdateGuildValidator : AbstractValidator<UpdateGuild>
    {
        public UpdateGuildValidator()
        {
            RuleFor(x => x.GuildId).NotEmpty().WithMessage("GuildId is required");
            RuleFor(x => x.UpdatedBy).NotEmpty().WithMessage("UpdatedBy is required");
            RuleFor(x => x.Name).MaximumLength(100).WithMessage("Guild name must not exceed 100 characters")
                .When(x => x.Name != null);
        }
    }
}
