using MediatR;
using Service.Lib.BaseResponse;

namespace Channel.Application.Features.Server.Commands.JoinServer;

public class JoinServerByInviteLink: IRequest<ApiResponse<Guid>>
{
    public string Code { get; set; }
    
    public Guid UserId { get; set; }

    public JoinServerByInviteLink() {}

    public JoinServerByInviteLink(string code, Guid userId)
    {
        Code = code;
        UserId = userId;
    }
}