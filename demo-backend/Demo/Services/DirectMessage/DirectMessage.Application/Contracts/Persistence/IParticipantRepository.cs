using DirectMessage.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectMessage.Application.Contracts.Persistence
{
    public interface IParticipantRepository
    {
        Task AddAsync(DirectMessageParticipant message);
        Task<IEnumerable<DirectMessageParticipant>> CheckUserConversation(Guid userId);
        Task<DirectMessageParticipant> CheckUserConversation(Guid userId, Guid conversationId);
    }
}
