using Authorize.Application.Contracts.Persistence;
using Authorize.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Service.Lib.BaseResponse;
using Service.Lib.Keycloak;
using System.Text.RegularExpressions;
using Grpc.Core;

namespace Authorize.Application.Features.Login.Commands.LoginCommand
{
    public class LoginHandler : IRequestHandler<Login, ApiResponse<TokenResponse>>
    {
        private readonly IRefreshTokenRepository _repository;
        private readonly IKeycloakService _keycloakService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginHandler(IRefreshTokenRepository repository, IKeycloakService keycloakService, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _keycloakService = keycloakService;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ApiResponse<TokenResponse>> Handle(Login request, CancellationToken cancellationToken)
        {
            try
            {
                var (newAccessToken, newRefreshToken) = await _keycloakService.GetUserTokenWithRefreshAsync(request.UserName, request.Password);

                if (string.IsNullOrEmpty(newAccessToken))
                    return ApiResponse<TokenResponse>.Failure("401", "Invalid Username or Password");
                
                // var refreshToken = new Authorize.Domain.Entities.RefreshToken
                // {
                //     UserId = Guid.Parse(user.Id),
                //     Token = newRefreshToken,
                //     ExpiresAt = DateTime.UtcNow.AddDays(7),
                //     IpAddress = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString(),
                //     UserAgent = _httpContextAccessor.HttpContext?.Request?.Headers["User-Agent"].ToString()
                // };
                // await _repository.AddAsync(refreshToken);

                return ApiResponse<TokenResponse>.Success(new TokenResponse
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken,
                }, "Login successful");
            }
            catch (RpcException ex)
            {
                return ApiResponse<TokenResponse>.Failure("404", "User not found");
            }
            catch (Exception ex)
            {
                return ApiResponse<TokenResponse>.Failure("500", "Internal Server Error");
            }
        }

        public bool IsEmail(string input)
        {
            return Regex.IsMatch(input, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }
    }
}
