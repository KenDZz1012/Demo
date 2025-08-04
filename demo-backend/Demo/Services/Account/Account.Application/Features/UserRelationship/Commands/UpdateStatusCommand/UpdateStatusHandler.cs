using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Account.Application.Contracts.Persistence;
using Account.Domain.Common.Constants;
using AutoMapper;
using Azure.Core;
using MediatR;
using Service.Lib.BaseResponse;

namespace Account.Application.Features.UserRelationship.Commands.UpdateStatusCommand
{
    public class UpdateStatusHandler : IRequestHandler<UpdateStatus, ApiResponse<UpdateStatusResponse>>
    {
        private readonly IUserRelationshipRepository _userRelationshipRepository;
        private readonly IMapper _mapper;
        private readonly HttpClient _httpClient;
        private readonly IUserRepository _userRepository;

        public UpdateStatusHandler(IUserRelationshipRepository userRelationshipRepository, IMapper mapper, IHttpClientFactory httpClientFactory, IUserRepository userRepository)
        {
            _userRelationshipRepository = userRelationshipRepository;
            _mapper = mapper;
            _httpClient = httpClientFactory.CreateClient("PresenceService");
            _userRepository = userRepository;
        }
        public async Task<ApiResponse<UpdateStatusResponse>> Handle(UpdateStatus request, CancellationToken cancellationToken)
        {
            try
            {
                var userRelationship = await _userRelationshipRepository.CheckExistRelationship(request.UserID, request.FriendID);
                var requester = await _userRepository.GetByIdAsync(request.UserID);
                var addressee = await _userRepository.GetByIdAsync(request.FriendID);
                if (userRelationship != null)
                {
                    userRelationship.Status = request.Status;
                    var isUpdatedSuccess = await _userRelationshipRepository.UpdateAsync(userRelationship);
                    if(!isUpdatedSuccess) return ApiResponse<UpdateStatusResponse>.Failure("500", "Update failed");
                    if(request.Status == UserRelationshipStatus.Accepted)
                    {
                        _ = NotifyFriendAccepted(requester, addressee.UserName);
                    }
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

        private async Task NotifyFriendAccepted(Domain.Entities.User fromUser, string toUserName)
        {
            var payload = new
            {
                FromUserName = fromUser.UserName,
                FromUserId = fromUser.Id,
                FromUserAvatarUrl = fromUser.AvatarUrl,
                FromUserDisplayName = fromUser.DisplayName,
                ToUserName = toUserName
            };
            try
            {
                Console.WriteLine(JsonSerializer.Serialize(payload, new JsonSerializerOptions { WriteIndented = true }));
                var response = await _httpClient.PostAsJsonAsync("v1/presence/friend-request-accepted", payload);
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"[Warning] Failed: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] Error: {ex.Message}");
            }
        }
        
        public async Task<Dictionary<string, bool>> GetOnlineStatusesAsync(List<string> userIds)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("v1/presence/batch-status",  userIds );
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadFromJsonAsync<Dictionary<string, bool>>();
                return result ?? new Dictionary<string, bool>();
            }
            catch
            {
                // Log error nếu cần
                return new Dictionary<string, bool>();
            }
        }
    }
}
