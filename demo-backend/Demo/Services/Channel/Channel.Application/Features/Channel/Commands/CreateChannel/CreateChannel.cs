using Channel.Domain.Common.Constants;
using MediatR;
using Service.Lib.BaseResponse;

namespace Channel.Application.Features.Channel.Commands.CreateChannel
{
    public class CreateChannel : IRequest<ApiResponse<Guid>>
    {
        public Guid GuildId { get; set; }

        public Guid? CategoryId { get; set; }

        public string Name { get; set; } = null!;

        public string Type { get; set; } = ChannelType.Text;

        public int Position { get; set; } = 0;

        public string? Topic { get; set; }
    }
}
