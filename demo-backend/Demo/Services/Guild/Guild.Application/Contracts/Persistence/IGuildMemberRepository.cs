using Guild.Domain.Entities;

namespace Guild.Application.Contracts.Persistence
{
    public interface IGuildMemberRepository
    {
        Task<List<GuildMember>> GetByGuildIdAsync(Guid guildId);
        Task<GuildMember?> GetAsync(Guid guildId, Guid userId);
        Task<bool> AddAsync(GuildMember member);
        Task<bool> DeleteAsync(GuildMember member);
    }
}
