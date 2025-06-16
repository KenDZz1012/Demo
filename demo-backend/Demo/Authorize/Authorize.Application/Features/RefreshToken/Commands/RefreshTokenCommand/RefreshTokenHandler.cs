using Authorize.Application.Contracts.Persistence;
using Authorize.Application.Models;
using AutoMapper;
using MediatR;
using Newtonsoft.Json.Linq;
using Service.Lib.BaseResponse;
using Service.Lib.Keycloak;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorize.Application.Features.RefreshToken.Commands.RefreshTokenCommand
{
    public class RefreshTokenHandler : IRequestHandler<RefreshToken, ApiResponse<TokenResponse>>
    {
        private readonly IRefreshTokenRepository _repository;
        private readonly IMapper _mapper;
        private readonly IKeycloakService _keycloakService;

        public RefreshTokenHandler(IRefreshTokenRepository repository, IMapper mapper, IKeycloakService keycloakService)
        {
            _repository = repository;
            _mapper = mapper;
            _keycloakService = keycloakService;
        }

        public async Task<ApiResponse<TokenResponse>> Handle(RefreshToken request, CancellationToken cancellationToken)
        {
            try
            {
                var filter = _mapper.Map<RefreshTokenFilter>(request);
                var storedToken = await _repository.GetRefreshTokenAsync(filter);
                if ((storedToken.IsRevoked.HasValue && storedToken.IsRevoked.Value) || storedToken.ExpiresAt < DateTime.UtcNow)
                    return await Task.FromResult(ApiResponse<TokenResponse>.Failure("401", "Invalid or expired refresh token"));

                var newToken = await _keycloakService.RefreshTokenAsync(request.refreshToken);

                storedToken.RevokedAt = DateTime.UtcNow;
                storedToken.IsRevoked = true;
                storedToken.ReplacedByToken = newToken.RefreshToken;

                await _repository.UpdateAsync(storedToken);

                await _repository.AddAsync(new Authorize.Domain.Entities.RefreshToken
                {
                    UserId = request.userID,
                    Token = newToken.RefreshToken,
                    ExpiresAt = DateTime.UtcNow.AddDays(7),
                    CreatedAt = DateTime.UtcNow,
                    IsRevoked = false
                });

                return ApiResponse<TokenResponse>.Success(new TokenResponse
                {
                    AccessToken = newToken.AccessToken,
                    RefreshToken = newToken.RefreshToken,
                    UserID = request.userID,
                }, "Refresh token successful");
            }
            catch (Exception ex)
            {
                return await Task.FromResult(ApiResponse<TokenResponse>.Failure("500", ex.Message));
            }
        }
    }
}
