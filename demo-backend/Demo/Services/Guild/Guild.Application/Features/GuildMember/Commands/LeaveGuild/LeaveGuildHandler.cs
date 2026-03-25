using Guild.Application.Contracts.Persistence;
using MediatR;
using Service.Lib.BaseResponse;

namespace Guild.Application.Features.GuildMember.Commands.LeaveGuild
{
    public class LeaveGuildHandler : IRequestHandler<LeaveGuild, ApiResponse<bool>>
    {
        private readonly IGuildRepository _guildRepository;
        private readonly IGuildMemberRepository _memberRepository;

        public LeaveGuildHandler(IGuildRepository guildRepository, IGuildMemberRepository memberRepository)
        {
            _guildRepository = guildRepository;
            _memberRepository = memberRepository;
        }

        public async Task<ApiResponse<bool>> Handle(LeaveGuild request, CancellationToken cancellationToken)
        {
            try
            {
                var guild = await _guildRepository.GetByIdAsync(request.GuildId);
                if (guild == null)
                    return ApiResponse<bool>.Failure("404", "Guild not found");

                if (guild.OwnerId == request.UserId)
                    return ApiResponse<bool>.Failure("403", "Owner cannot leave the guild. Transfer ownership or delete the guild first");

                var member = await _memberRepository.GetAsync(request.GuildId, request.UserId);
                if (member == null)
                    return ApiResponse<bool>.Failure("404", "User is not a member of this guild");

                member.DeletedBy = request.UserId;
                var isLeft = await _memberRepository.DeleteAsync(member);
                return isLeft
                    ? ApiResponse<bool>.Success(true, "Left guild successfully")
                    : ApiResponse<bool>.Failure("500", "Failed to leave guild");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Failure("500", ex.Message);
            }
        }
    }
}
