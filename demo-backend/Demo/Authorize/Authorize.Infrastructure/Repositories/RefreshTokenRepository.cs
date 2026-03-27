using Authorize.Application.Contracts.Persistence;
using Authorize.Domain.Entities;
using Authorize.Infrastructure.Data;
using Service.Lib.BaseRepository.PostgreSQL;


namespace Authorize.Infrastructure.Repositories
{
    public class RefreshTokenRepository : BaseRepository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(AuthorizeContext context) : base(context) { }

        /// <summary>
        /// Thêm mới RefreshToken
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(RefreshToken refreshToken)
        {
            await base.AddAsync(refreshToken);
            return await base.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Cập nhật RefreshToken
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(RefreshToken refreshToken)
        {
            base.Update(refreshToken);
            return await base.SaveChangesAsync() > 0;
        }
        
    }
}
