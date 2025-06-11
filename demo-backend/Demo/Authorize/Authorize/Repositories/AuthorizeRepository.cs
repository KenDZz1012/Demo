using Authorize.Model;
using Service.Lib.Context;
using Service.Lib.Password;
using Dapper;
using Service.Lib.JWT;
using Service.Lib.Keycloak;
using Azure;
using Service.Lib.BaseResponse;
using Newtonsoft.Json.Linq;

namespace Authorize.Repositories
{
    public class AuthorizeRepository : IAuthorizeRepository
    {
        private readonly DapperContext _dapperContext;
        private readonly IKeycloakService _keycloakService;
        public AuthorizeRepository(DapperContext dapperContext, IKeycloakService keycloakService)
        {
            _dapperContext = dapperContext;
            _keycloakService = keycloakService;
        }

        public async Task<ApiResponse<TokenResponse>> Authorization(Login login, HttpResponse response)
        {
            try
            {
                var token = await _keycloakService.GetUserTokenWithRefreshAsync(login.UserName, login.Password);
                if (string.IsNullOrEmpty(token.AccessToken))
                    return ApiResponse<TokenResponse>.Failure("401", "Invalid Username or Password");

                return ApiResponse<TokenResponse>.Success(new TokenResponse()
                {
                    AccessToken = token.AccessToken,
                    RefreshToken = token.RefreshToken,
                }, "Login successfull");
            }
            catch (Exception ex)
            {
                return ApiResponse<TokenResponse>.Failure("401", "Invalid Username or Password");
            }
        }


        public async Task<ApiResponse<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request)
        {
            if (string.IsNullOrEmpty(request.RefreshToken))
                return ApiResponse<TokenResponse>.Failure("401", "Unauthorized");

            var newTokens = await _keycloakService.RefreshTokenAsync(request.RefreshToken);
            if (string.IsNullOrEmpty(newTokens.AccessToken))
                return ApiResponse<TokenResponse>.Failure("401", "Unauthorized");

            return ApiResponse<TokenResponse>.Success(new TokenResponse()
            {
                AccessToken = newTokens.AccessToken,
                RefreshToken = newTokens.RefreshToken,
            }, "Successfull");
        }
    }
}
