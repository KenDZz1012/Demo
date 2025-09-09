using Authorize.Application.Contracts.Persistence;
using Authorize.Application.Models;
using Authorize.Domain.Entities;
using Authorize.Infrastructure.Data;
using Service.Lib.BaseRepository.PostgreSQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<RefreshToken> GetRefreshTokenAsync(RefreshTokenFilter filter)
        {
            var queryBuilder = Query();
            queryBuilder.Filter(u => u.Token == filter.RefreshToken && u.UserId == filter.UserId);
            queryBuilder.Sort("CreatedAt", false);
            return await queryBuilder.FirstOrDefaultAsync();
        }
    }
}
