using FluentValidation;

namespace Guild.Application.Features.GuildMember.Commands.JoinGuild
{
    public class JoinGuildValidator : AbstractValidator<JoinGuild>
    {
        public JoinGuildValidator()
        {
            RuleFor(x => x.InviteCode).NotEmpty().WithMessage("Invite code is required");
            RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required");
        }
    }
}
