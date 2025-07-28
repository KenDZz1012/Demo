using Channel.Application.Contracts.Persistence;
using MediatR;
using Service.Lib.BaseResponse;

namespace Channel.Application.Features.ServerMember.Commands.LeaveServer;

public class LeaveServerHandler : IRequestHandler<LeaveServer, ApiResponse<bool>>
{
    private readonly IServerMemberRepository _serverMemberRepository;

    public LeaveServerHandler(IServerMemberRepository serverMemberRepository)
    {
        _serverMemberRepository = serverMemberRepository;
    }
    
    public async Task<ApiResponse<bool>> Handle(LeaveServer request, CancellationToken cancellationToken)
    {
        try
        {
            var existingMember = await _serverMemberRepository.CheckUserMemberExistInServer(request.ServerId, request.UserId);
            if(existingMember == null) return ApiResponse<bool>.Failure("404", "Not found member in server");
            var isDeleted = await _serverMemberRepository.DeleteAsync(existingMember);
            return isDeleted ? ApiResponse<bool>.Success(isDeleted, "Leave server) successfully")
                             : ApiResponse<bool>.Failure("500", "Leave server failed");   
        }
        catch (Exception ex)
        {
            return await Task.FromResult(ApiResponse<bool>.Failure("500", ex.Message));
        }
    }
}