using GuildEntity = Guild.Domain.Entities.Guild;

namespace Guild.Application.Contracts.Persistence
{
    public interface IGuildRepository
    {
        Task<List<GuildEntity>> GetByUserIdAsync(Guid userId);
        Task<GuildEntity?> GetByIdAsync(Guid id);
        Task<bool> AddAsync(GuildEntity guild);
        Task<bool> UpdateAsync(GuildEntity guild);
        Task<bool> DeleteAsync(GuildEntity guild);
    }
}
