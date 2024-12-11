using Authorize.Model;
using Service.Lib.Context;
using Service.Lib.Password;
using Dapper;
using Service.Lib.JWT;

namespace Authorize.Repositories
{
    public class AuthorizeRepository : IAuthorizeRepository
    {
        private readonly DapperContext _dapperContext;
        public AuthorizeRepository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<Login> Authorization(Login login)
        {
            login.PasswordWeb = await PasswordMD5.CreateMD5(login.PasswordWeb);
            string sql = " Select UserID, UserName from tbl_User Where UserID = @UserID and PasswordWeb = @PasswordWeb ";

            using (var connection = _dapperContext.CreateConnection())
            {
                var user = await connection.QuerySingleAsync<Login>(sql, login);
                if (user != null)
                {
                    user.Token = Jwt.GenerateToken(user.UserID).ToString();
                }
                return user;
            }
        }
    }
}
