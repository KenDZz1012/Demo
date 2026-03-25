using FluentValidation;

namespace Channel.Application.Features.Channel.Commands.DeleteChannel
{
    public class DeleteChannelValidator : AbstractValidator<DeleteChannel>
    {
        public DeleteChannelValidator()
        {
            RuleFor(x => x.ChannelId).NotEmpty().WithMessage("ChannelId is required");
            RuleFor(x => x.DeletedBy).NotEmpty().WithMessage("DeletedBy is required");
        }
    }
}
