using Authorize.Application.Contracts.Persistence;
using Authorize.Application.Models;
using AutoMapper;
using MediatR;
using Newtonsoft.Json;
using Service.Lib.BaseResponse;
using Service.Lib.Keycloak;


namespace Authorize.Application.Features.RefreshToken.Commands.RefreshTokenCommand
{
    public class RefreshTokenHandler : IRequestHandler<RefreshToken, ApiResponse<RefreshTokenResponse>>
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

        public async Task<ApiResponse<RefreshTokenResponse>> Handle(RefreshToken request, CancellationToken cancellationToken)
        {
            try
            {
                var filter = _mapper.Map<RefreshTokenFilter>(request);
                var storedToken = await _repository.GetRefreshTokenAsync(filter);
                Console.WriteLine(JsonConvert.SerializeObject(storedToken, Formatting.Indented));
                if ((storedToken.IsRevoked.HasValue && storedToken.IsRevoked.Value) || storedToken.ExpiresAt < DateTime.UtcNow)
                    return await Task.FromResult(ApiResponse<RefreshTokenResponse>.Failure("401", "Invalid or expired refresh token"));

                var newToken = await _keycloakService.RefreshTokenAsync(request.refreshToken);

                storedToken.RevokedAt = DateTime.UtcNow;
                storedToken.IsRevoked = true;
                storedToken.ReplacedByToken = newToken.RefreshToken;
                
                Console.WriteLine($"Refresh token for user {request.userID} is being updated. New access token: {newToken.AccessToken}, New refresh token: {newToken.RefreshToken}");

                await _repository.UpdateAsync(storedToken);

                await _repository.AddAsync(new Authorize.Domain.Entities.RefreshToken
                {
                    UserId = request.userID,
                    Token = newToken.RefreshToken,
                    ExpiresAt = DateTime.UtcNow.AddDays(7),
                    CreatedAt = DateTime.UtcNow,
                    IsRevoked = false
                });

                return ApiResponse<RefreshTokenResponse>.Success(new RefreshTokenResponse
                {
                    AccessToken = newToken.AccessToken,
                    RefreshToken = newToken.RefreshToken,
                }, "Refresh token successful");
            }
            catch (Exception ex)
            {
                return await Task.FromResult(ApiResponse<RefreshTokenResponse>.Failure("500", ex.Message));
            }
        }
    }
}
