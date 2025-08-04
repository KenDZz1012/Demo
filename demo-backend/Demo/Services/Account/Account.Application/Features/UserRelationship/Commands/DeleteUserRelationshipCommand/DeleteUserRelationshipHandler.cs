using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Account.Application.Contracts.Persistence;
using AutoMapper;
using Azure.Core;
using MediatR;
using Service.Lib.BaseResponse;

namespace Account.Application.Features.UserRelationship.Commands.DeleteUserRelationshipCommand
{
    public class DeleteUserRelationshipHandler : IRequestHandler<DeleteUserRelationship, ApiResponse<Guid>>
    {
        private readonly IUserRelationshipRepository _userRelationshipRepository;
        private readonly IMapper _mapper;
        private readonly HttpClient _httpClient;
        private readonly IUserRepository _userRepository;
        public DeleteUserRelationshipHandler(IUserRelationshipRepository userRelationshipRepository, IMapper mapper, IUserRepository userRepository, IHttpClientFactory httpClientFactory)
        {
            _userRelationshipRepository = userRelationshipRepository;
            _mapper = mapper;
            _httpClient = httpClientFactory.CreateClient("PresenceService");
            _userRepository = userRepository;
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
                    var requester = await _userRepository.GetByIdAsync(request.UserID);
                    var addressee = await _userRepository.GetByIdAsync(request.FriendID);
                    _ = NotifyFriendRejected(requester, addressee.UserName);
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

        private async Task NotifyFriendRejected(Domain.Entities.User fromUser, string toUserName)
        {
            var payload = new
            {
                FromUserName = fromUser.UserName,
                FromUserId = fromUser.Id,
                ToUserName = toUserName
            };
            try
            {
                Console.WriteLine(JsonSerializer.Serialize(payload, new JsonSerializerOptions { WriteIndented = true }));
                var response = await _httpClient.PostAsJsonAsync("v1/presence/fiend-request-rejected", payload);
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
    }
}
