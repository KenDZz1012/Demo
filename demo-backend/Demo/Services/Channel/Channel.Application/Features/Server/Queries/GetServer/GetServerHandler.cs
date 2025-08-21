using AutoMapper;
using Channel.Application.Contracts.Persistence;
using Channel.Application.Features.Server.Queries.GetServers;
using MediatR;
using Service.Lib.BaseResponse;

namespace Channel.Application.Features.Server.Queries.GetServer;

public class GetServerHandler: IRequestHandler<GetServer, ApiResponse<GetServerVm>>
{
    public readonly IServerRepository _serverRepository;
    public readonly IMapper _mapper;

    public GetServerHandler(IServerRepository serverRepository, IMapper mapper)
    {
        _mapper = mapper;
        _serverRepository = serverRepository;
    }

    public async Task<ApiResponse<GetServerVm>> Handle(GetServer request, CancellationToken cancellationToken)
    {
        try
        {
            var server = await _serverRepository.GetServer(request.ServerId);
            var serverDto = _mapper.Map<GetServerVm>(server);
            return ApiResponse<GetServerVm>.Success(serverDto, "Get list server successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<GetServerVm>.Failure("500", ex.Message);
        }
    }
}