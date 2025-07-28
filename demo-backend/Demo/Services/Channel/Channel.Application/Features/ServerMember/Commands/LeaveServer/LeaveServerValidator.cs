using FluentValidation;

namespace Channel.Application.Features.ServerMember.Commands.LeaveServer;

public class LeaveServerValidator : AbstractValidator<LeaveServer>
{
    public LeaveServerValidator()
    {
        RuleFor(x => x.ServerId)
            .NotEmpty().NotNull()
            .WithMessage("Tên server không được trống");

        RuleFor(x => x.UserId)
            .NotEmpty().NotNull()
            .WithMessage("User không được trống");
    }
}