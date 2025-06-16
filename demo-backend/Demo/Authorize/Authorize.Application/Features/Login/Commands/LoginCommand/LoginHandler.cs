using Authorize.Application.Contracts.Persistence;
using Authorize.Application.Models;
using Authorize.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Service.Lib.BaseResponse;
using Service.Lib.HttpRequest;
using Service.Lib.Keycloak;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Authorize.Application.Features.Login.Commands.LoginCommand
{
    public class LoginHandler : IRequestHandler<Login, ApiResponse<TokenResponse>>
    {
        private readonly IRefreshTokenRepository _repository;
        private readonly IKeycloakService _keycloakService;
        private readonly IHttpRequestService _httpRequestService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginHandler(IRefreshTokenRepository repository, IKeycloakService keycloakService, IHttpRequestService httpRequestService, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _keycloakService = keycloakService;
            _httpRequestService = httpRequestService;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ApiResponse<TokenResponse>> Handle(Login request, CancellationToken cancellationToken)
        {
            try
            {
                var token = await _keycloakService.GetUserTokenWithRefreshAsync(request.UserName, request.Password);

                if (string.IsNullOrEmpty(token.AccessToken))
                    return ApiResponse<TokenResponse>.Failure("401", "Invalid Username or Password");
                var userResponse = await GetUserFromApiAsync(request.UserName);

                if (!userResponse.IsSuccess || userResponse.Data is not { Count: > 0 })
                    return ApiResponse<TokenResponse>.Failure("404", "User not found");

                var user = userResponse.Data.First();
                var refreshToken = new Authorize.Domain.Entities.RefreshToken
                {
                    UserId = user.ID,
                    Token = token.RefreshToken,
                    ExpiresAt = DateTime.UtcNow.AddDays(7),
                    IpAddress = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString(),
                    UserAgent = _httpContextAccessor.HttpContext?.Request?.Headers["User-Agent"].ToString()
                };
                await _repository.AddAsync(refreshToken);

                return ApiResponse<TokenResponse>.Success(new TokenResponse
                {
                    AccessToken = token.AccessToken,
                    RefreshToken = token.RefreshToken,
                    UserID = user.ID
                }, "Login successful");
            }
            catch (Exception ex)
            {
                return ApiResponse<TokenResponse>.Failure("401", "Invalid Username or Password");
            }
        }

        public bool IsEmail(string input)
        {
            return Regex.IsMatch(input, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        private async Task<ApiResponse<List<UserResponse>>> GetUserFromApiAsync(string userInput)
        {
            var queryParam = IsEmail(userInput) ? $"Email={userInput}" : $"UserName={userInput}";
            return await _httpRequestService.GetAsync<ApiResponse<List<UserResponse>>>(
                "http://103.82.25.49/kong-gw", $"/acc/User?{queryParam}", null);
        }
    }
}
