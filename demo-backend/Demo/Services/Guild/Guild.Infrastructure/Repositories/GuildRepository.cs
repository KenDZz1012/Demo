using Guild.Application.Contracts.Persistence;
using Guild.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Service.Lib.BaseRepository.PostgreSQL;
using GuildEntity = Guild.Domain.Entities.Guild;

namespace Guild.Infrastructure.Repositories
{
    public class GuildRepository : BaseRepository<GuildEntity>, IGuildRepository
    {
        private readonly GuildContext _context;

        public GuildRepository(GuildContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<GuildEntity>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Guilds
                .Where(g => g.DeletedAt == null &&
                    _context.GuildMembers.Any(m => m.GuildId == g.Id && m.UserId == userId && m.DeletedAt == null))
                .ToListAsync();
        }

        public async Task<GuildEntity?> GetByIdAsync(Guid id)
        {
            return await _context.Guilds
                .Include(g => g.GuildMembers.Where(m => m.DeletedAt == null))
                .FirstOrDefaultAsync(g => g.Id == id && g.DeletedAt == null);
        }

        public async Task<bool> AddAsync(GuildEntity guild)
        {
            await base.AddAsync(guild);
            return await base.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(GuildEntity guild)
        {
            _context.Guilds.Update(guild);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(GuildEntity guild)
        {
            guild.DeletedAt = DateTime.UtcNow;
            _context.Guilds.Update(guild);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
