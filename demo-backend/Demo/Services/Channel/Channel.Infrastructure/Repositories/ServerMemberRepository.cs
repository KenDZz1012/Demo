using Channel.Application.Contracts.Persistence;
using Channel.Domain.Entities;
using Channel.Infrastructure.Data;
using Service.Lib.BaseRepository;

namespace Channel.Infrastructure.Repositories
{
    public class ServerMemberRepository: BaseRepository<ServerMember>,IServerMemberRepository
    {
        public ServerMemberRepository(ChannelContext context) : base(context)
        {
        }
        
        /// <summary>
        /// Thêm thành viên vào server
        /// </summary>
        /// <param name="serverMember"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(ServerMember serverMember)
        {
            await base.AddAsync(serverMember);
            return await base.SaveChangesAsync() > 0;
        }
    }
}
