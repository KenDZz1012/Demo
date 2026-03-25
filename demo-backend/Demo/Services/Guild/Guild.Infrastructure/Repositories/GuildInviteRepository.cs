using Guild.Application.Contracts.Persistence;
using Guild.Domain.Entities;
using Guild.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Service.Lib.BaseRepository.PostgreSQL;

namespace Guild.Infrastructure.Repositories
{
    public class GuildInviteRepository : BaseRepository<GuildInvite>, IGuildInviteRepository
    {
        private readonly GuildContext _context;

        public GuildInviteRepository(GuildContext context) : base(context)
        {
            _context = context;
        }

        public async Task<GuildInvite?> GetByCodeAsync(string code)
        {
            return await _context.GuildInvites
                .FirstOrDefaultAsync(i => i.Code == code &&
                    (i.ExpiresAt == null || i.ExpiresAt > DateTime.UtcNow) &&
                    (i.MaxUses == 0 || i.Uses < i.MaxUses));
        }

        public async Task<bool> AddAsync(GuildInvite invite)
        {
            await base.AddAsync(invite);
            return await base.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(GuildInvite invite)
        {
            _context.GuildInvites.Update(invite);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
