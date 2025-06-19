using Authorize.Application.Contracts.Persistence;
using Authorize.Application.Models;
using Authorize.Domain.Entities;
using Authorize.GrpcServices;
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
        private readonly UserGrpcService _userGrpcService;

        public LoginHandler(IRefreshTokenRepository repository, IKeycloakService keycloakService, IHttpRequestService httpRequestService, IHttpContextAccessor httpContextAccessor, UserGrpcService userGrpcService)
        {
            _repository = repository;
            _keycloakService = keycloakService;
            _httpRequestService = httpRequestService;
            _httpContextAccessor = httpContextAccessor;
            _userGrpcService = userGrpcService;
        }
        public async Task<ApiResponse<TokenResponse>> Handle(Login request, CancellationToken cancellationToken)
        {
            try
            {
                var (newAccessToken, newRefreshToken) = await _keycloakService.GetUserTokenWithRefreshAsync(request.UserName, request.Password);

                if (string.IsNullOrEmpty(newAccessToken))
                    return ApiResponse<TokenResponse>.Failure("401", "Invalid Username or Password");

                var user = await _userGrpcService.GetUserByUserNameOrEmailAsync(request.UserName);

                var refreshToken = new Authorize.Domain.Entities.RefreshToken
                {
                    UserId = Guid.Parse(user.Id),
                    Token = newRefreshToken,
                    ExpiresAt = DateTime.UtcNow.AddDays(7),
                    IpAddress = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString(),
                    UserAgent = _httpContextAccessor.HttpContext?.Request?.Headers["User-Agent"].ToString()
                };
                await _repository.AddAsync(refreshToken);

                return ApiResponse<TokenResponse>.Success(new TokenResponse
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken,
                    UserID = Guid.Parse(user.Id),
                }, "Login successful");
            }
            catch (Exception ex)
            {
                return ApiResponse<TokenResponse>.Failure("401", ex.Message);
            }
        }

        public bool IsEmail(string input)
        {
            return Regex.IsMatch(input, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }
    }
}
