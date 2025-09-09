using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectMessage.Application.Contracts.Persistence
{
    public interface IDirectMessageRepository
    {
        Task AddAsync(DirectMessage.Domain.Entities.DirectMessage message);
        Task UpdateAsync(DirectMessage.Domain.Entities.DirectMessage message);
        Task DeleteAsync(DirectMessage.Domain.Entities.DirectMessage message);
    }
}
