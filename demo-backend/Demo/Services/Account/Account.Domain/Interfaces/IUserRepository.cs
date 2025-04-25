using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Account.Domain.Entities;
using Account.Domain.Filters;
using Account.Domain.Model.User;

namespace Account.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync(UserFilter userFilter);
        Task<User> GetByIdAsync(Guid id);
        Task AddAsync(IUserCreateInfo user);
        Task UpdateAsync(User user);
        Task DeleteAsync(Guid id);
    }
}
