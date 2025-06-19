using Account.Grpc.Context;
using Microsoft.AspNetCore.Http.Extensions;
using Service.Lib.BaseRepository;
using Service.Lib.QueryBuilder;

namespace Account.Grpc.Repositories
{
    public class UserRepository :  BaseRepository<User>, IUserRepository
    {
        public UserRepository(UserContext context) : base(context)
        {
        }
        public async Task<User> GetUserByUserNameOrEmail(string search)
        {
            var queryBuilder = Query();
            if (!string.IsNullOrEmpty(search)) queryBuilder.Filter(u => u.UserName == search || u.Email == search);
            return await queryBuilder.FirstOrDefaultAsync();
        }
    }
}
