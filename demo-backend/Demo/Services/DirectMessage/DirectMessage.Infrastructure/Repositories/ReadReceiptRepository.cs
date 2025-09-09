using DirectMessage.Application.Contracts.Persistence;
using DirectMessage.Domain.Entities;
using DirectMessage.Infrastructure.Data;
using Service.Lib.BaseRepository.ScyllaDB;

namespace DirectMessage.Infrastructure.Repositories;

public class ReadReceiptRepository : BaseRepositoryScyllaDB<DirectMessageReadReceipt>, IReadReceiptRepository
{
    public ReadReceiptRepository(DirectMessageContext context) : base(context) { }

    public async Task AddAsync(DirectMessageReadReceipt message)
    {
        await base.InsertAsync(message);
    }

    public async Task UpdateAsync(DirectMessageReadReceipt message)
    {
        await base.UpdateAsync(message);
    }

    public async Task DeleteMessageAsync(DirectMessageReadReceipt message)
    {
        await base.DeleteAsync(message);
    }
}