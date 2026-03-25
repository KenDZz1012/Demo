using Guild.Application.Contracts.Persistence;
using Guild.Domain.Entities;
using MediatR;
using Service.Lib.BaseResponse;

namespace Guild.Application.Features.GuildMember.Commands.JoinGuild
{
    public class JoinGuildHandler : IRequestHandler<JoinGuild, ApiResponse<bool>>
    {
        private readonly IGuildInviteRepository _inviteRepository;
        private readonly IGuildMemberRepository _memberRepository;

        public JoinGuildHandler(IGuildInviteRepository inviteRepository, IGuildMemberRepository memberRepository)
        {
            _inviteRepository = inviteRepository;
            _memberRepository = memberRepository;
        }

        public async Task<ApiResponse<bool>> Handle(JoinGuild request, CancellationToken cancellationToken)
        {
            try
            {
                var invite = await _inviteRepository.GetByCodeAsync(request.InviteCode);
                if (invite == null)
                    return ApiResponse<bool>.Failure("404", "Invite code is invalid or expired");

                var existing = await _memberRepository.GetAsync(invite.GuildId, request.UserId);
                if (existing != null)
                    return ApiResponse<bool>.Failure("409", "User is already a member of this guild");

                invite.Uses += 1;
                await _inviteRepository.UpdateAsync(invite);

                var member = new Domain.Entities.GuildMember
                {
                    GuildId = invite.GuildId,
                    UserId = request.UserId,
                    JoinedAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = request.UserId
                };
                var isAdded = await _memberRepository.AddAsync(member);
                return isAdded
                    ? ApiResponse<bool>.Success(true, "Joined guild successfully")
                    : ApiResponse<bool>.Failure("500", "Failed to join guild");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Failure("500", ex.Message);
            }
        }
    }
}
