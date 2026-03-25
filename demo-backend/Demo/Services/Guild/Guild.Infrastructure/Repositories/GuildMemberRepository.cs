using Guild.Application.Contracts.Persistence;
using Guild.Domain.Entities;
using Guild.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Service.Lib.BaseRepository.PostgreSQL;

namespace Guild.Infrastructure.Repositories
{
    public class GuildMemberRepository : BaseRepository<GuildMember>, IGuildMemberRepository
    {
        private readonly GuildContext _context;

        public GuildMemberRepository(GuildContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<GuildMember>> GetByGuildIdAsync(Guid guildId)
        {
            return await _context.GuildMembers
                .Where(m => m.GuildId == guildId && m.DeletedAt == null)
                .OrderBy(m => m.JoinedAt)
                .ToListAsync();
        }

        public async Task<GuildMember?> GetAsync(Guid guildId, Guid userId)
        {
            return await _context.GuildMembers
                .FirstOrDefaultAsync(m => m.GuildId == guildId && m.UserId == userId && m.DeletedAt == null);
        }

        public async Task<bool> AddAsync(GuildMember member)
        {
            await base.AddAsync(member);
            return await base.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(GuildMember member)
        {
            member.DeletedAt = DateTime.UtcNow;
            _context.GuildMembers.Update(member);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
