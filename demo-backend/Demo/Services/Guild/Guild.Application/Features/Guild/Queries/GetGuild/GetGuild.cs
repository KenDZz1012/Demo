using MediatR;
using Service.Lib.BaseResponse;

namespace Guild.Application.Features.Guild.Queries.GetGuild
{
    public class GetGuild : IRequest<ApiResponse<GetGuildVm>>
    {
        public Guid GuildId { get; set; }

        public GetGuild(Guid guildId)
        {
            GuildId = guildId;
        }
    }
}
