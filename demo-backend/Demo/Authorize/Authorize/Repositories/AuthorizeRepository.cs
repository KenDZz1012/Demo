using Authorize.Model;
using Service.Lib.Context;
using Service.Lib.Password;
using Dapper;
using Service.Lib.JWT;
using Service.Lib.Keycloak;
using Azure;

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

        public async Task<bool> Authorization(Login login, HttpResponse response)
        {
            try
            {
                var token = await _keycloakService.GetUserTokenWithRefreshAsync(login.UserName, login.Password);
                if (string.IsNullOrEmpty(token.AccessToken))
                {
                    return false;
                }

                if (string.IsNullOrEmpty(token.AccessToken))
                    return false;

                SetTokenCookies(response, token.AccessToken, token.RefreshToken);

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }


        public async Task<bool> RefreshTokenAsync(HttpRequest request, HttpResponse response)
        {
            if (!request.Cookies.TryGetValue("refresh_token", out var refreshToken))
                return false;

            var newTokens = await _keycloakService.RefreshTokenAsync(refreshToken);
            if (string.IsNullOrEmpty(newTokens.AccessToken))
                return false;

            SetTokenCookies(response, newTokens.AccessToken, newTokens.RefreshToken);
            return true;
        }

        public Task LogoutAsync(HttpResponse response)
        {
            // Xóa cookie
            response.Cookies.Delete("access_token");
            response.Cookies.Delete("refresh_token");
            return Task.CompletedTask;
        }

        private void SetTokenCookies(HttpResponse response, string accessToken, string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddMinutes(15)
            };

            response.Cookies.Append("access_token", accessToken, cookieOptions);

            var refreshOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            };

            response.Cookies.Append("refresh_token", refreshToken, refreshOptions);
        }
    }
}
