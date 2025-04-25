using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Account.Application.DTOs.UserRelationship;
using Account.Domain.Entities;

namespace Account.Application.Interfaces
{
    public interface IUserRelationshipService
    {
        Task<List<UserRelationshipDTO>> GetAllAsync();
        Task<UserRelationshipDTO> GetByIdAsync(Guid id);
        Task AddAsync(UserRelationship user);
        Task UpdateAsync(UserRelationship user);
        Task DeleteAsync(Guid id);
    }
}
