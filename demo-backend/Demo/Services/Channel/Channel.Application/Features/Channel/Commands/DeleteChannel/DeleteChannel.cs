using MediatR;
using Service.Lib.BaseResponse;

namespace Channel.Application.Features.Channel.Commands.DeleteChannel
{
    public class DeleteChannel : IRequest<ApiResponse<bool>>
    {
        public Guid ChannelId { get; set; }
        public Guid DeletedBy { get; set; }

        public DeleteChannel(Guid channelId, Guid deletedBy)
        {
            ChannelId = channelId;
            DeletedBy = deletedBy;
        }
    }
}
