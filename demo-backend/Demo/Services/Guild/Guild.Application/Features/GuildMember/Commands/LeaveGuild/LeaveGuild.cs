using MediatR;
using Service.Lib.BaseResponse;

namespace Guild.Application.Features.GuildMember.Commands.LeaveGuild
{
    public class LeaveGuild : IRequest<ApiResponse<bool>>
    {
        public Guid GuildId { get; set; }
        public Guid UserId { get; set; }
    }
}
