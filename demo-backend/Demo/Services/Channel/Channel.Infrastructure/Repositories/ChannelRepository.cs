using Channel.Application.Contracts.Persistence;
using Channel.Infrastructure.Data;
using Service.Lib.BaseRepository.PostgreSQL;

namespace Channel.Infrastructure.Repositories
{
    public class ChannelRepository: BaseRepository<Domain.Entities.Channel>, IChannelRepository
    {
        public ChannelRepository(ChannelContext context) : base(context)
        {
        }
        
        /// <summary>
        /// Thêm channel
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(Domain.Entities.Channel channel)
        {
            await base.AddAsync(channel);
            return await base.SaveChangesAsync() > 0;
        }
    }
}
