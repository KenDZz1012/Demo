using FluentValidation;

namespace Channel.Application.Features.Channel.Commands.CreateChannel
{
    public class CreateChannelValidator : AbstractValidator<CreateChannel>
    {
        public CreateChannelValidator()
        {
            RuleFor(x => x.GuildId).NotEmpty().WithMessage("GuildId is required");
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Channel name is required")
                .MaximumLength(100).WithMessage("Channel name must not exceed 100 characters");
        }
    }
}
