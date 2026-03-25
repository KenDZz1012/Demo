using Channel.Application.Contracts.Persistence;
using Channel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Service.Lib.BaseRepository.PostgreSQL;

namespace Channel.Infrastructure.Repositories
{
    public class ChannelRepository : BaseRepository<Domain.Entities.Channel>, IChannelRepository
    {
        private readonly ChannelContext _context;

        public ChannelRepository(ChannelContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Domain.Entities.Channel>> GetByGuildIdAsync(Guid guildId)
        {
            return await _context.Channels
                .Where(c => c.GuildId == guildId && c.DeletedAt == null)
                .OrderBy(c => c.Position)
                .ToListAsync();
        }

        public async Task<Domain.Entities.Channel?> GetByIdAsync(Guid id)
        {
            return await _context.Channels
                .FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null);
        }

        public async Task<bool> AddAsync(Domain.Entities.Channel channel)
        {
            await base.AddAsync(channel);
            return await base.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(Domain.Entities.Channel channel)
        {
            _context.Channels.Update(channel);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Domain.Entities.Channel channel)
        {
            channel.DeletedAt = DateTime.UtcNow;
            _context.Channels.Update(channel);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
