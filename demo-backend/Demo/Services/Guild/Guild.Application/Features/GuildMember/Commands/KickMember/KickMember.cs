using MediatR;
using Service.Lib.BaseResponse;

namespace Guild.Application.Features.GuildMember.Commands.KickMember
{
    public class KickMember : IRequest<ApiResponse<bool>>
    {
        public Guid GuildId { get; set; }
        public Guid TargetUserId { get; set; }
        public Guid KickedBy { get; set; }
    }
}
