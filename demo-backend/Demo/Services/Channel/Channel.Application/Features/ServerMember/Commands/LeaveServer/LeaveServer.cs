using MediatR;
using Service.Lib.BaseResponse;

namespace Channel.Application.Features.ServerMember.Commands.LeaveServer;

public class LeaveServer : IRequest<ApiResponse<bool>>
{
    public Guid ServerId { get; set; }
    public Guid UserId { get; set; }

    public LeaveServer() { }
    
    public LeaveServer(Guid serverId, Guid userId)
    {
        ServerId = serverId;
        UserId = userId;
    }
}