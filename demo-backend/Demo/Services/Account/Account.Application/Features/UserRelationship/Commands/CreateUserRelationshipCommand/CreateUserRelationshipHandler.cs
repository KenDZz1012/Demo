using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Account.Application.Contracts.Persistence;
using AutoMapper;
using MediatR;
using Service.Lib.BaseResponse;
using Service.Lib.Password;

namespace Account.Application.Features.UserRelationship.Commands.CreateUserRelationshipCommand
{
    public class CreateUserRelationshipHandler : IRequestHandler<CreateUserRelationship, ApiResponse<CreateUserRelationshipResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserRelationshipRepository _userRelationshipRepository;
        private readonly IMapper _mapper;
        private readonly HttpClient _httpClient;

        public CreateUserRelationshipHandler(IUserRelationshipRepository userRelationshipRepository, IMapper mapper, IUserRepository userRepository, IHttpClientFactory httpClientFactory)
        {
            _userRelationshipRepository = userRelationshipRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _httpClient = httpClientFactory.CreateClient("PresenceService");
        }
        public async Task<ApiResponse<CreateUserRelationshipResponse>> Handle(CreateUserRelationship request, CancellationToken cancellationToken)
        {
            try
            {
                var addressee = await _userRepository.CheckExistUserName(request.AddresseeName);
                if (addressee == null)
                    return ApiResponse<CreateUserRelationshipResponse>.Failure("404", "User does not exist");

                var requester = await _userRepository.GetByIdAsync(request.RequesterId);
                if (requester == null)
                    return ApiResponse<CreateUserRelationshipResponse>.Failure("404", "User does not exist");

                if (requester.UserName == request.AddresseeName)
                    return ApiResponse<CreateUserRelationshipResponse>.Failure("400", "Cannot send friend request to yourself");

                var existing = await _userRelationshipRepository.CheckExistRelationship(request.RequesterId, addressee.Id);
                if (existing != null)
                    return ApiResponse<CreateUserRelationshipResponse>.Failure("409", "Relationship already exists");

                var userRelationship = _mapper.Map<Account.Domain.Entities.UserRelationship>(request);
                userRelationship.AddresseeId = addressee.Id;

                var isCreated = await _userRelationshipRepository.AddAsync(userRelationship);
                if (!isCreated)
                    return ApiResponse<CreateUserRelationshipResponse>.Failure("500", "Failed to create relationship");

                _ = NotifyPresenceServiceAsync(requester, addressee.UserName);
                var userReceived = _mapper.Map<CreateUserRelationshipResponse>(addressee);
                return ApiResponse<CreateUserRelationshipResponse>.Success(userReceived, "Friend request sent");
            }
            catch (Exception ex)
            {
                return ApiResponse<CreateUserRelationshipResponse>.Failure("500", ex.Message);
            }
        }
        
        private async Task NotifyPresenceServiceAsync(Domain.Entities.User fromUser, string toUserName)
        {
            var payload = new { 
                FromUserName = fromUser.UserName, 
                FromUserId = fromUser.Id, 
                FromUserAvatarUrl = fromUser.AvatarUrl, 
                FromUserDisplayName = fromUser.DisplayName, 
                ToUserName = toUserName
            };            
            try
            {
                Console.WriteLine(JsonSerializer.Serialize(payload, new JsonSerializerOptions { WriteIndented = true }));
                var response = await _httpClient.PostAsJsonAsync("v1/presence/friend-request", payload);
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"[Warning] NotifyPresenceService failed: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error] NotifyPresenceService error: {ex.Message}");
            }
        }

    }
}


