using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Account.Application.Features.UserRelationship.Queries.GetListUserRelationshipQuery;
using Account.Domain.Entities;

namespace Account.Application.Contracts.Persistence
{
    public interface IUserRelationshipRepository
    {
        Task<List<UserRelationship>> GetAllAsync(GetListUserRelationship filter);
        Task<UserRelationship> GetByIdAsync(Guid id);
        Task<bool> AddAsync(UserRelationship userRelationship);
        Task<bool> UpdateAsync(UserRelationship userRelationship);
        Task<bool> DeleteAsync(UserRelationship userRelationship);
        Task<UserRelationship> CheckExistRelationship(Guid requesterId, Guid addresseeId);
        Task<List<UserRelationship>> GetUserRelationships(Guid userId);
    }
}
