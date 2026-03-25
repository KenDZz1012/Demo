using MediatR;
using Service.Lib.BaseResponse;

namespace Guild.Application.Features.Guild.Commands.DeleteGuild
{
    public class DeleteGuild : IRequest<ApiResponse<bool>>
    {
        public Guid GuildId { get; set; }
        public Guid DeletedBy { get; set; }

        public DeleteGuild(Guid guildId, Guid deletedBy)
        {
            GuildId = guildId;
            DeletedBy = deletedBy;
        }
    }
}
