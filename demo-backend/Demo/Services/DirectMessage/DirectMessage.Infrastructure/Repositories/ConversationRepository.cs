using DirectMessage.Application.Contracts.Persistence;
using DirectMessage.Domain.Entities;
using DirectMessage.Infrastructure.Data;
using Service.Lib.BaseRepository.ScyllaDB;

namespace DirectMessage.Infrastructure.Repositories;

public class ConversationRepository : BaseRepositoryScyllaDB<DirectMessageConversation>, IConversationRepository
{
    public ConversationRepository(DirectMessageContext context) : base(context) { }

    public async Task AddAsync(DirectMessageConversation message)
    {
        await base.InsertAsync(message);
    }

    public async Task UpdateAsync(DirectMessageConversation message)
    {
        await base.UpdateAsync(message);
    }

    public async Task DeleteMessageAsync(DirectMessageConversation message)
    {
        await base.DeleteAsync(message);
    }

    public async Task<DirectMessageConversation> CheckExistConversation(Guid converationId)
    {
        return await base.FirstOrDefaultAsync("conversation_id = ?", converationId);
    }
}