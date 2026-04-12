using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Account.Application.Contracts.Persistence;
using Account.Domain.Common.Constants;
using AutoMapper;
using MediatR;
using Service.Lib.BaseResponse;

namespace Account.Application.Features.UserRelationship.Commands.UpdateStatusCommand
{
    public class UpdateStatusHandler : IRequestHandler<UpdateStatus, ApiResponse<UpdateStatusResponse>>
    {
        private readonly IUserRelationshipRepository _userRelationshipRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public UpdateStatusHandler(IUserRelationshipRepository userRelationshipRepository, IMapper mapper, IUserRepository userRepository)
        {
            _userRelationshipRepository = userRelationshipRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }
        public async Task<ApiResponse<UpdateStatusResponse>> Handle(UpdateStatus request, CancellationToken cancellationToken)
        {
            try
            {
                var userRelationship = await _userRelationshipRepository.CheckExistRelationship(request.UserID, request.FriendID);
                var addressee = await _userRepository.GetByIdAsync(request.FriendID);
                if (userRelationship != null)
                {
                    userRelationship.Status = request.Status;
                    var isUpdatedSuccess = await _userRelationshipRepository.UpdateAsync(userRelationship);
                    if(!isUpdatedSuccess) return ApiResponse<UpdateStatusResponse>.Failure("500", "Update failed");
                    var response = _mapper.Map<UpdateStatusResponse>(addressee);
                    var onlineStatuses = await GetOnlineStatusesAsync(new List<string> { response.UserName });
                    response.IsOnline = onlineStatuses.TryGetValue(response.UserName, out var isOnline) && isOnline;
                    return ApiResponse<UpdateStatusResponse>.Success(response, "Update successfully");
                }
                else
                {
                    return ApiResponse<UpdateStatusResponse>.Failure("500", "Not exist");
                }
            }
            catch (Exception ex)
            {
                return await Task.FromResult(ApiResponse<UpdateStatusResponse>.Failure("500", ex.Message));
            }
        }

        public Task<Dictionary<string, bool>> GetOnlineStatusesAsync(List<string> userIds) =>
            Task.FromResult(new Dictionary<string, bool>());
    }
}
