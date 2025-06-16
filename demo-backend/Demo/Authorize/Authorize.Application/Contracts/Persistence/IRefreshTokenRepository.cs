using Authorize.Application.Models;
using Authorize.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorize.Application.Contracts.Persistence
{
    public interface IRefreshTokenRepository
    {
        Task<bool> AddAsync(RefreshToken refreshToken);
        Task<bool> UpdateAsync(RefreshToken refreshToken);
        Task<RefreshToken> GetRefreshTokenAsync(RefreshTokenFilter filter);
    }
}
