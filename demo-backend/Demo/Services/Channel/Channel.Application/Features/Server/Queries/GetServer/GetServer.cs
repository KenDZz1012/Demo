using MediatR;
using Service.Lib.BaseResponse;

namespace Channel.Application.Features.Server.Queries.GetServer;

public class GetServer: IRequest<ApiResponse<GetServerVm>>
{
    public Guid ServerId { get; set; }

    public GetServer() {} 
        
    public GetServer(Guid serverId)
    {
        ServerId = serverId;
    }
}