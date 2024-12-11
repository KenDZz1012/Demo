using Authorize.Model;

namespace Authorize.Repositories
{
    public interface IAuthorizeRepository
    {
        Task<Login> Authorization(Login login);
    }
}
