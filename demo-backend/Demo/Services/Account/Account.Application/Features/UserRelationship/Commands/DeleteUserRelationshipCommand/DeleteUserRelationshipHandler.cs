using System;
using System.Threading.Tasks;
using Account.Application.Contracts.Persistence;
using MediatR;
using Service.Lib.BaseResponse;

namespace Account.Application.Features.UserRelationship.Commands.DeleteUserRelationshipCommand
{
    public class DeleteUserRelationshipHandler : IRequestHandler<DeleteUserRelationship, ApiResponse<Guid>>
    {
        private readonly IUserRelationshipRepository _userRelationshipRepository;

        public DeleteUserRelationshipHandler(IUserRelationshipRepository userRelationshipRepository)
        {
            _userRelationshipRepository = userRelationshipRepository;
        }
        public async Task<ApiResponse<Guid>> Handle(DeleteUserRelationship request, CancellationToken cancellationToken)
        {
            try
            {
                var exitRelationship = await _userRelationshipRepository.CheckExistRelationship(request.UserID,request.FriendID);
                if (exitRelationship != null)
                {
                    var isDeletedSuccess = await _userRelationshipRepository.DeleteAsync(exitRelationship);
                    if(!isDeletedSuccess) return ApiResponse<Guid>.Failure("500", "Delete failed");
                    return ApiResponse<Guid>.Success(exitRelationship.Id, "Delete successfully!");
                }
                else
                {
                    return ApiResponse<Guid>.Failure("500", "Not exist relationship");
                }
            }
            catch(Exception ex)
            {
                return await Task.FromResult(ApiResponse<Guid>.Failure("500", ex.Message));

            }
        }
    }
}
