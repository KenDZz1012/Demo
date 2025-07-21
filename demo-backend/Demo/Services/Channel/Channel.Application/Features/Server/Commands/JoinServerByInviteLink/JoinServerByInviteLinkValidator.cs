using FluentValidation;

namespace Channel.Application.Features.Server.Commands.JoinServer;

public class JoinServerByInviteLinkValidator : AbstractValidator<JoinServerByInviteLink>
{
    public JoinServerByInviteLinkValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().NotNull()
            .WithMessage("Mã mời không được trống");

        RuleFor(x => x.UserId)
            .NotEmpty().NotNull()
            .WithMessage("Mã User không được trống");
    }
}