using Guild.Application.Contracts.Persistence;
using MediatR;
using Service.Lib.BaseResponse;

namespace Guild.Application.Features.GuildMember.Commands.KickMember
{
    public class KickMemberHandler : IRequestHandler<KickMember, ApiResponse<bool>>
    {
        private readonly IGuildRepository _guildRepository;
        private readonly IGuildMemberRepository _memberRepository;

        public KickMemberHandler(IGuildRepository guildRepository, IGuildMemberRepository memberRepository)
        {
            _guildRepository = guildRepository;
            _memberRepository = memberRepository;
        }

        public async Task<ApiResponse<bool>> Handle(KickMember request, CancellationToken cancellationToken)
        {
            try
            {
                var guild = await _guildRepository.GetByIdAsync(request.GuildId);
                if (guild == null)
                    return ApiResponse<bool>.Failure("404", "Guild not found");

                if (guild.OwnerId != request.KickedBy)
                    return ApiResponse<bool>.Failure("403", "Only the guild owner can kick members");

                if (request.TargetUserId == request.KickedBy)
                    return ApiResponse<bool>.Failure("400", "Cannot kick yourself");

                var member = await _memberRepository.GetAsync(request.GuildId, request.TargetUserId);
                if (member == null)
                    return ApiResponse<bool>.Failure("404", "Member not found in guild");

                member.DeletedBy = request.KickedBy;
                var isKicked = await _memberRepository.DeleteAsync(member);
                return isKicked
                    ? ApiResponse<bool>.Success(true, "Member kicked successfully")
                    : ApiResponse<bool>.Failure("500", "Failed to kick member");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Failure("500", ex.Message);
            }
        }
    }
}
