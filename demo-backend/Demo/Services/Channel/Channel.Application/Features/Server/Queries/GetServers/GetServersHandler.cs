using MediatR;
using Service.Lib.BaseResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Channel.Application.Contracts.Persistence;

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
        public async Task<ApiResponse<List<GetServersVm>>> Handle(GetServers request, CancellationToken cancellationToken)
        {
            try
            {
                var servers = await _serverRepository.GetServers(request);
                return ApiResponse<List<GetServersVm>>.Success(_mapper.Map<List<GetServersVm>>(servers), "Get list server successfully");
            }
            catch(Exception ex)
            {
                return ApiResponse<List<GetServersVm>>.Failure("500", ex.Message);
            }
        }
    }
}
