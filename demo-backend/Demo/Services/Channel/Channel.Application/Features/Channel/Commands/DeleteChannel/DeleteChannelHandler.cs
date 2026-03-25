using Channel.Application.Contracts.Persistence;
using MediatR;
using Service.Lib.BaseResponse;

namespace Channel.Application.Features.Channel.Commands.DeleteChannel
{
    public class DeleteChannelHandler : IRequestHandler<DeleteChannel, ApiResponse<bool>>
    {
        private readonly IChannelRepository _channelRepository;

        public DeleteChannelHandler(IChannelRepository channelRepository)
        {
            _channelRepository = channelRepository;
        }

        public async Task<ApiResponse<bool>> Handle(DeleteChannel request, CancellationToken cancellationToken)
        {
            try
            {
                var channel = await _channelRepository.GetByIdAsync(request.ChannelId);
                if (channel == null)
                    return ApiResponse<bool>.Failure("404", "Channel not found");

                channel.DeletedBy = request.DeletedBy;
                var isDeleted = await _channelRepository.DeleteAsync(channel);
                return isDeleted
                    ? ApiResponse<bool>.Success(true, "Delete channel successfully")
                    : ApiResponse<bool>.Failure("500", "Delete channel failed");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Failure("500", ex.Message);
            }
        }
    }
}
