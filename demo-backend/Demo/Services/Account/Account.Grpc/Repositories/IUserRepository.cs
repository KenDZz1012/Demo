using Account.Grpc.Context;

namespace Account.Grpc.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByUserNameOrEmail(string search);
    }
}
