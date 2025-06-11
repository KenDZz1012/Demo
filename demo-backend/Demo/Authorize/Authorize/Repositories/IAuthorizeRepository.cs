using Authorize.Model;
using Service.Lib.BaseResponse;

namespace Authorize.Repositories
{
    public interface IAuthorizeRepository
    {
        Task<ApiResponse<TokenResponse>> Authorization(Login login, HttpResponse response);
        Task<ApiResponse<TokenResponse>> RefreshTokenAsync(HttpRequest request, HttpResponse response);
    }
}
