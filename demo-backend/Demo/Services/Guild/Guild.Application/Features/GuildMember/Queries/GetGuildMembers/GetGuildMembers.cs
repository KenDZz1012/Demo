using MediatR;
using Service.Lib.BaseResponse;

namespace Guild.Application.Features.GuildMember.Queries.GetGuildMembers
{
    public class GetGuildMembers : IRequest<ApiResponse<List<GetGuildMembersVm>>>
    {
        public Guid GuildId { get; set; }

        public GetGuildMembers(Guid guildId)
        {
            GuildId = guildId;
        }
    }
}
