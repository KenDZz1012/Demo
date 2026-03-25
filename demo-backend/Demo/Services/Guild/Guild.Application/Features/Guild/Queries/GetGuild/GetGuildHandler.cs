using AutoMapper;
using Guild.Application.Contracts.Persistence;
using MediatR;
using Service.Lib.BaseResponse;

namespace Guild.Application.Features.Guild.Queries.GetGuild
{
    public class GetGuildHandler : IRequestHandler<GetGuild, ApiResponse<GetGuildVm>>
    {
        private readonly IGuildRepository _guildRepository;
        private readonly IMapper _mapper;

        public GetGuildHandler(IGuildRepository guildRepository, IMapper mapper)
        {
            _guildRepository = guildRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<GetGuildVm>> Handle(GetGuild request, CancellationToken cancellationToken)
        {
            try
            {
                var guild = await _guildRepository.GetByIdAsync(request.GuildId);
                if (guild == null)
                    return ApiResponse<GetGuildVm>.Failure("404", "Guild not found");

                var vm = _mapper.Map<GetGuildVm>(guild);
                vm.MemberCount = guild.GuildMembers.Count;
                return ApiResponse<GetGuildVm>.Success(vm, "Get guild successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<GetGuildVm>.Failure("500", ex.Message);
            }
        }
    }
}
