using Channel.Application.Contracts.Persistence;
using Channel.Domain.Entities;
using Channel.Infrastructure.Data;
using Service.Lib.BaseRepository.PostgreSQL;

namespace Channel.Infrastructure.Repositories;

public class ServerInviteLinkRepository: BaseRepository<ServerInviteLink>, IServerInviteLinkRepository
{
    public ServerInviteLinkRepository(ChannelContext context) : base(context)
    {
    }
    
    /// <summary>
    /// Thêm ServerInviteLink
    /// </summary>
    /// <param name="server"></param>
    /// <returns></returns>
    public async Task<bool> AddAsync(ServerInviteLink server)
    {
        await base.AddAsync(server);
        return await base.SaveChangesAsync() > 0;
    }
    
    /// <summary>
    /// Lấy ServerInviteLink theo ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<ServerInviteLink> GetByIdAsync(Guid id)
    {
        return await base.GetByIdAsync(id);
    }

    /// <summary>
    /// Check code tồn tại
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    public async Task<ServerInviteLink> CheckExistCode(string code)
    {
        var queryBuilder = Query();
        queryBuilder.Filter(u => u.Code == code && (u.ExpiresAt > DateTime.UtcNow || u.ExpiresAt == null));
        return await queryBuilder.FirstOrDefaultAsync();
    }
    
    
}