using Authorize.Application.Contracts.Persistence;
using Authorize.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Service.Lib.BaseResponse;
using System.Text.RegularExpressions;

namespace Authorize.Application.Features.Login.Commands.LoginCommand
{
    public class LoginHandler : IRequestHandler<Login, ApiResponse<TokenResponse>>
    {
        private readonly IRefreshTokenRepository _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginHandler(IRefreshTokenRepository repository,IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ApiResponse<TokenResponse>> Handle(Login request, CancellationToken cancellationToken)
        {
            try
            {
                var (newAccessToken, newRefreshToken) = await _keycloakService.GetUserTokenWithRefreshAsync(request.UserName, request.Password);

                if (string.IsNullOrEmpty(newAccessToken))
                    return ApiResponse<TokenResponse>.Failure("401", "Invalid Username or Password");

                return ApiResponse<TokenResponse>.Success(new TokenResponse
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken,
                }, "Login successful");
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
