namespace Channel.Application.Contracts.Persistence
{
    public interface IChannelRepository
    {
        Task<List<Domain.Entities.Channel>> GetByGuildIdAsync(Guid guildId);
        Task<Domain.Entities.Channel?> GetByIdAsync(Guid id);
        Task<bool> AddAsync(Domain.Entities.Channel channel);
        Task<bool> UpdateAsync(Domain.Entities.Channel channel);
        Task<bool> DeleteAsync(Domain.Entities.Channel channel);
    }
}
