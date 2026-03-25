using AutoMapper;
using Guild.Application.Contracts.Persistence;
using MediatR;
using Service.Lib.BaseResponse;

namespace Guild.Application.Features.Guild.Queries.GetGuilds
{
    public class GetGuildsHandler : IRequestHandler<GetGuilds, ApiResponse<List<GetGuildsVm>>>
    {
        private readonly IGuildRepository _guildRepository;
        private readonly IMapper _mapper;

        public GetGuildsHandler(IGuildRepository guildRepository, IMapper mapper)
        {
            _guildRepository = guildRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<GetGuildsVm>>> Handle(GetGuilds request, CancellationToken cancellationToken)
        {
            try
            {
                var guilds = await _guildRepository.GetByUserIdAsync(request.UserId);
                var result = _mapper.Map<List<GetGuildsVm>>(guilds);
                return ApiResponse<List<GetGuildsVm>>.Success(result, "Get guilds successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<GetGuildsVm>>.Failure("500", ex.Message);
            }
        }
    }
}
