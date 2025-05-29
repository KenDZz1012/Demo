using Channel.Application.Contracts.Persistence;
using Channel.Application.Models.Server;
using Channel.Infrastructure.Data;
using Service.Lib.BaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public async Task<List<Server>> GetServers(Filter filter)
        {
            var queryBuilder = Query();
            if (filter.OwnerId != null) queryBuilder.Filter(u => u.OwnerId == filter.OwnerId);
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
    }
}
