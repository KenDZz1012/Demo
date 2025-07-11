using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Channel.Application.Contracts.Persistence;
using MediatR;
using Service.Lib.BaseResponse;
using Service.Lib.Minio;

namespace Channel.Application.Features.Channel.Commands.CreateChannel
{
    public class CreateChannelHandler : IRequestHandler<CreateChannel,ApiResponse<Guid>>
    {
        private readonly IChannelRepository _channelRepository;
        private readonly IMapper _mapper;
        private readonly IServerRepository _serverRepository;
        public CreateChannelHandler(IChannelRepository channelRepository, IMapper mapper, IServerRepository serverRepository)
        {
            _channelRepository = channelRepository;
            _mapper = mapper;
            _serverRepository = serverRepository;
        }
        public async Task<ApiResponse<Guid>> Handle(CreateChannel request, CancellationToken cancellationToken)
        {
            try
            {
                var existingServer = await _serverRepository.GetServer(request.ServerId);
                if (existingServer == null) return ApiResponse<Guid>.Failure("404", "Server not found");
                var channel = _mapper.Map<Domain.Entities.Channel>(request);
                var isCreatedSuccess = await _channelRepository.AddAsync(channel);
                return isCreatedSuccess
                    ? ApiResponse<Guid>.Success(channel.Id, "Create channel successfully")
                    : ApiResponse<Guid>.Failure("500", "Create channel failed");
            }
            catch (Exception ex)
            {
                return await Task.FromResult(ApiResponse<Guid>.Failure("500", ex.Message));
            }
        }
    }
}
