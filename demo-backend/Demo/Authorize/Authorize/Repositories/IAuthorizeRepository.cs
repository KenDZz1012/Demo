using Authorize.Model;
using Service.Lib.BaseResponse;

namespace Authorize.Repositories
{
    public interface IAuthorizeRepository
    {
        Task<ApiResponse<bool>> Authorization(Login login, HttpResponse response);
        Task<ApiResponse<bool>> RefreshTokenAsync(HttpRequest request, HttpResponse response);
        Task LogoutAsync(HttpResponse response);
    }
}
