using Channel.Application.Contracts.Persistence;
using Channel.Domain.Common.Constants;
using MediatR;
using Service.Lib.BaseResponse;

namespace Channel.Application.Features.Server.Commands.JoinServer;

public class JoinServerByInviteLinkHandler : IRequestHandler<JoinServerByInviteLink, ApiResponse<Guid>>
{
    private readonly IServerRepository _serverRepository;
    private readonly IServerInviteLinkRepository _serverInviteLinkRepository;
    private readonly IServerMemberRepository _serverMemberRepository;

    public JoinServerByInviteLinkHandler(IServerRepository serverRepository,
        IServerInviteLinkRepository serverInviteLinkRepository, IServerMemberRepository serverMemberRepository)
    {
        _serverMemberRepository = serverMemberRepository;
        _serverRepository = serverRepository;
        _serverInviteLinkRepository = serverInviteLinkRepository;
    }
    
    public async Task<ApiResponse<Guid>> Handle(JoinServerByInviteLink request, CancellationToken cancellationToken)
    {
        try
        {
            var serverInviteLink = await _serverInviteLinkRepository.CheckExistCode(request.Code);
            if(serverInviteLink == null)
            {
                return ApiResponse<Guid>.Failure("404", "Invite link not found");
            }
            var existingMember = await _serverMemberRepository.CheckUserExistInServer(serverInviteLink.ServerId, request.UserId);
            if (existingMember != null)
            {
                return ApiResponse<Guid>.Failure("400", "User already exists in the server");
            }
            
            var serverMember = new Domain.Entities.ServerMember
            {
                ServerId = serverInviteLink.ServerId,
                UserId = request.UserId,
                Role = ServerMemberRole.Member
            };
            await _serverMemberRepository.AddAsync(serverMember); 
            return ApiResponse<Guid>.Success(serverInviteLink.ServerId, "Create server successfully");
        }
        catch (Exception ex)
        {
            return ApiResponse<Guid>.Failure("500", ex.Message);
        }
    }
}