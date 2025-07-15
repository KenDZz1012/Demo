using Channel.Application.Features.Server.Queries.GetServers;
using Channel.Domain.Entities;

namespace Channel.Application.Contracts.Persistence
{
    public interface IServerRepository
    {
        Task<List<Server>> GetServers(GetServers filter);
        Task<Server> GetServer(Guid Id);
        Task<bool> AddAsync(Server server);
        Task<bool> DeleteAsync(Server server);
        Task<Server> CheckServerExist(Guid serverId);
    }
}
