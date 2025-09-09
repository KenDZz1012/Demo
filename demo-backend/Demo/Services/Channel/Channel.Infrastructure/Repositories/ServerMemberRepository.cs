using Channel.Application.Contracts.Persistence;
using Channel.Domain.Common.Constants;
using Channel.Domain.Entities;
using Channel.Infrastructure.Data;
using Service.Lib.BaseRepository.PostgreSQL;

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

        /// <summary>
        /// Check xem người dùng đã tồn tại trong server hay chưa
        /// </summary>
        /// <param name="serverId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ServerMember> CheckUserExistInServer(Guid serverId, Guid userId)
        {
            var queryBuilder = Query();
            queryBuilder.Filter(x => x.ServerId == serverId && x.UserId == userId);
            return await queryBuilder.FirstOrDefaultAsync();
        }
        
        
        /// <summary>
        /// Check xem người dùng đã tồn tại trong server hay chưa
        /// </summary>
        /// <param name="serverId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ServerMember> CheckUserMemberExistInServer(Guid serverId, Guid userId)
        {
            var queryBuilder = Query();
            queryBuilder.Filter(x => x.ServerId == serverId && x.UserId == userId && x.Role == ServerMemberRole.Member);
            return await queryBuilder.FirstOrDefaultAsync();
        }
        
        /// <summary>
        /// Xóa member khỏi server
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(ServerMember serverMember)
        {
            base.Delete(serverMember);
            return await base.SaveChangesAsync() > 0;
        }
    }
}
