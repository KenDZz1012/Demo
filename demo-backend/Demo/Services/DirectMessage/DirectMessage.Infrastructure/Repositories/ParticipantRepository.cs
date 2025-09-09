using DirectMessage.Application.Contracts.Persistence;
using DirectMessage.Domain.Entities;
using DirectMessage.Infrastructure.Data;
using Service.Lib.BaseRepository.ScyllaDB;

namespace DirectMessage.Infrastructure.Repositories;

public class ParticipantRepository : BaseRepositoryScyllaDB<DirectMessageParticipant>, IParticipantRepository
{
    public ParticipantRepository(DirectMessageContext context) : base(context) { }

    public async Task AddAsync(DirectMessageParticipant message)
    {
        await base.InsertAsync(message);
    }

    public async Task UpdateAsync(DirectMessageParticipant message)
    {
        await base.UpdateAsync(message);
    }

    public async Task DeleteMessageAsync(DirectMessageParticipant message)
    {
        await base.DeleteAsync(message);
    }

    public async Task<IEnumerable<DirectMessageParticipant>> CheckUserConversation(Guid userId)
    {
        return await base.GetWhereAsync("user_id = ?", userId);
    }

    public async Task<DirectMessageParticipant> CheckUserConversation(Guid userId, Guid conversationId)
    {
        return await base.FirstOrDefaultAsync("conversation_id = ? and user_id = ?", conversationId, userId);
    }
}