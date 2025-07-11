using Channel.Application.Contracts.Persistence;
using Channel.Application.Features.Server.Queries.GetServers;
using Channel.Domain.Entities;
using Channel.Infrastructure.Data;
using Service.Lib.BaseRepository;

namespace Channel.Infrastructure.Repositories
{
    public class ServerRepository : BaseRepository<Server>, IServerRepository
    {
        public ServerRepository(AppDbContext context) : base(context) { }

        /// <summary>
        /// Lấy ra danh sách Server
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<List<Server>> GetServers(GetServers filter)
        {
            var queryBuilder = Query();
            if (filter.OwnerId != null) queryBuilder.Filter(u => u.OwnerId == filter.OwnerId || u.ServerMembers.Any(x=>x.UserId == filter.OwnerId));
            queryBuilder.Include(x=> x.Channels);
            queryBuilder.Include(x=> x.ServerMembers);
            return await queryBuilder.ToListAsync();
        }

        /// <summary>
        /// Thêm server
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(Server server)
        {
            await base.AddAsync(server);
            return await base.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Xóa server
        /// </summary>
        /// <param name="server"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(Server server)
        {
            base.Delete(server);
            return await base.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Lấy server theo ID
        /// </summary>
        /// <param name="serverId"></param>
        /// <returns></returns>
        public async Task<Server> GetServer(Guid serverId)
        {
            return await base.GetByIdAsync(serverId);
        }
    }
}
