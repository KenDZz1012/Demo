using DirectMessage.Infrastructure.Data;
using DirectMessage.Domain.Entities;
using DirectMessage.Application.Contracts.Persistence;
using Service.Lib.BaseRepository.ScyllaDB;

namespace DirectMessage.Infrastructure.Repositories;

public class DirectMessageRepository : BaseRepositoryScyllaDB<DirectMessage.Domain.Entities.DirectMessage>, IDirectMessageRepository
{

    public DirectMessageRepository(DirectMessageContext context) : base(context) { }

    public async Task AddAsync(DirectMessage.Domain.Entities.DirectMessage message)
    {
        await base.InsertAsync(message);
    }

    public async Task UpdateAsync(DirectMessage.Domain.Entities.DirectMessage message)
    {
        await base.UpdateAsync(message);
    }

    public async Task DeleteAsync(DirectMessage.Domain.Entities.DirectMessage message)
    {
        await base.DeleteAsync(message);
    }
}