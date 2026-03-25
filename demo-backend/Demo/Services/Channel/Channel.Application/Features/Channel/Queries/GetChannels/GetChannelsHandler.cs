using AutoMapper;
using Channel.Application.Contracts.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;
using Service.Lib.BaseResponse;

namespace Channel.Application.Features.Channel.Queries.GetChannels
{
    public class GetChannelsHandler : IRequestHandler<GetChannels, ApiResponse<List<GetChannelsVm>>>
    {
        private readonly IChannelRepository _channelRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetChannelsHandler> _logger;
        public GetChannelsHandler(IChannelRepository channelRepository, IMapper mapper, ILogger<GetChannelsHandler> logger)
        {
            _channelRepository = channelRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApiResponse<List<GetChannelsVm>>> Handle(GetChannels request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("GetChannels handler started");
                var channels = await _channelRepository.GetByGuildIdAsync(request.GuildId);
                var result = _mapper.Map<List<GetChannelsVm>>(channels);
                return ApiResponse<List<GetChannelsVm>>.Success(result, "Get channels successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<GetChannelsVm>>.Failure("500", ex.Message);
            }
        }
    }
}
