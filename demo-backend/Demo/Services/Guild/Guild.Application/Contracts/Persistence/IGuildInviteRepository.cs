using Guild.Domain.Entities;

namespace Guild.Application.Contracts.Persistence
{
    public interface IGuildInviteRepository
    {
        Task<GuildInvite?> GetByCodeAsync(string code);
        Task<bool> AddAsync(GuildInvite invite);
        Task<bool> UpdateAsync(GuildInvite invite);
    }
}
