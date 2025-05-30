using Authorize.Model;

namespace Authorize.Repositories
{
    public interface IAuthorizeRepository
    {
        Task<bool> Authorization(Login login, HttpResponse response);
        Task<bool> RefreshTokenAsync(HttpRequest request, HttpResponse response);
        Task LogoutAsync(HttpResponse response);
    }
}
