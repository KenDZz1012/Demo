using MediatR;
using Service.Lib.BaseResponse;

namespace Guild.Application.Features.GuildMember.Commands.JoinGuild
{
    public class JoinGuild : IRequest<ApiResponse<bool>>
    {
        public string InviteCode { get; set; } = null!;
        public Guid UserId { get; set; }
    }
}
