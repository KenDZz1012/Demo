using DirectMessage.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectMessage.Application.Contracts.Persistence
{
    public interface IConversationRepository
    {
        Task AddAsync(DirectMessageConversation message);
        Task<DirectMessageConversation> CheckExistConversation(Guid converationId);
    }
}
