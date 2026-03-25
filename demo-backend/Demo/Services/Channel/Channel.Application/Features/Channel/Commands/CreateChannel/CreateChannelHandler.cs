using AutoMapper;
using Channel.Application.Contracts.Persistence;
using MediatR;
using Service.Lib.BaseResponse;

namespace Channel.Application.Features.Channel.Commands.CreateChannel
{
    public class CreateChannelHandler : IRequestHandler<CreateChannel, ApiResponse<Guid>>
    {
        private readonly IChannelRepository _channelRepository;
        private readonly IMapper _mapper;

        public CreateChannelHandler(IChannelRepository channelRepository, IMapper mapper)
        {
            _channelRepository = channelRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<Guid>> Handle(CreateChannel request, CancellationToken cancellationToken)
        {
            try
            {
                var channel = _mapper.Map<Domain.Entities.Channel>(request);
                channel.CreatedAt = DateTime.UtcNow;
                var isCreated = await _channelRepository.AddAsync(channel);
                return isCreated
                    ? ApiResponse<Guid>.Success(channel.Id, "Create channel successfully")
                    : ApiResponse<Guid>.Failure("500", "Create channel failed");
            }
            catch (Exception ex)
            {
                return ApiResponse<Guid>.Failure("500", ex.Message);
            }
        }
    }
}
