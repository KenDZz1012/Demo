using Guild.Application.Contracts.Persistence;
using MediatR;
using Service.Lib.BaseResponse;

namespace Guild.Application.Features.Guild.Commands.DeleteGuild
{
    public class DeleteGuildHandler : IRequestHandler<DeleteGuild, ApiResponse<bool>>
    {
        private readonly IGuildRepository _guildRepository;

        public DeleteGuildHandler(IGuildRepository guildRepository)
        {
            _guildRepository = guildRepository;
        }

        public async Task<ApiResponse<bool>> Handle(DeleteGuild request, CancellationToken cancellationToken)
        {
            try
            {
                var guild = await _guildRepository.GetByIdAsync(request.GuildId);
                if (guild == null)
                    return ApiResponse<bool>.Failure("404", "Guild not found");

                guild.DeletedBy = request.DeletedBy;
                var isDeleted = await _guildRepository.DeleteAsync(guild);
                return isDeleted
                    ? ApiResponse<bool>.Success(true, "Delete guild successfully")
                    : ApiResponse<bool>.Failure("500", "Delete guild failed");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Failure("500", ex.Message);
            }
        }
    }
}
