using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Account.Domain.Entities;

namespace Account.Domain.Interfaces
{
    public interface IUserRelationshipRepository
    {
        Task<List<UserRelationship>> GetAllAsync();
        Task<UserRelationship> GetByIdAsync(Guid id);
        Task AddAsync(UserRelationship userRelationship);
        Task UpdateAsync(UserRelationship userRelationship);
        Task DeleteAsync(Guid id);
    }
}
