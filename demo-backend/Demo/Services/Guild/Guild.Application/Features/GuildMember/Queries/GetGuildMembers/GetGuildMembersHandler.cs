using AutoMapper;
using Guild.Application.Contracts.Persistence;
using MediatR;
using Service.Lib.BaseResponse;

namespace Guild.Application.Features.GuildMember.Queries.GetGuildMembers
{
    public class GetGuildMembersHandler : IRequestHandler<GetGuildMembers, ApiResponse<List<GetGuildMembersVm>>>
    {
        private readonly IGuildMemberRepository _memberRepository;
        private readonly IMapper _mapper;

        public GetGuildMembersHandler(IGuildMemberRepository memberRepository, IMapper mapper)
        {
            _memberRepository = memberRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<GetGuildMembersVm>>> Handle(GetGuildMembers request, CancellationToken cancellationToken)
        {
            try
            {
                var members = await _memberRepository.GetByGuildIdAsync(request.GuildId);
                var result = _mapper.Map<List<GetGuildMembersVm>>(members);
                return ApiResponse<List<GetGuildMembersVm>>.Success(result, "Get guild members successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<GetGuildMembersVm>>.Failure("500", ex.Message);
            }
        }
    }
}
