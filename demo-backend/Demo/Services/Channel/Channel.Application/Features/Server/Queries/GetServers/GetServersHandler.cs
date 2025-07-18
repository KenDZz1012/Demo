using MediatR;
using Service.Lib.BaseResponse;
using AutoMapper;
using Channel.Application.Contracts.Persistence;
using Channel.Application.GrpcServices;

namespace Channel.Application.Features.Server.Queries.GetServers
{
    public class GetServersHandler : IRequestHandler<GetServers, ApiResponse<List<GetServersVm>>>
    {
        public readonly IServerRepository _serverRepository;
        public readonly IMapper _mapper;

        public GetServersHandler(IServerRepository serverRepository, IMapper mapper)
        {
            _serverRepository = serverRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<GetServersVm>>> Handle(GetServers request,
            CancellationToken cancellationToken)
        {
            try
            {
                var servers = await _serverRepository.GetServers(request);
                var serverDto = _mapper.Map<List<GetServersVm>>(servers);
                return ApiResponse<List<GetServersVm>>.Success(serverDto, "Get list server successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<GetServersVm>>.Failure("500", ex.Message);
            }
        }
    }
}