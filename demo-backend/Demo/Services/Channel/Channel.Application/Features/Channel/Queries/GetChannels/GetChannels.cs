using MediatR;
using Service.Lib.BaseResponse;

namespace Channel.Application.Features.Channel.Queries.GetChannels
{
    public class GetChannels : IRequest<ApiResponse<List<GetChannelsVm>>>
    {
        public Guid GuildId { get; set; }

        public GetChannels(Guid guildId)
        {
            GuildId = guildId;
        }
    }
}
