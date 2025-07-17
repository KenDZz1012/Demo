using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Account.Application.Features.User.Queries.GetUsersQuery;
using Account.Domain.Entities;

namespace Account.Application.Contracts.Persistence
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync(GetUsers userFilter);
        Task<User> GetByIdAsync(Guid id);
        Task<bool> AddAsync(User user);
        Task<bool> UpdateAsync(User user);
        Task<bool> DeleteAsync(User user);
        Task<User> CheckExistUserName(string userName);
        Task<User> CheckExistEmail(string email);
        Task<User> GetUserByUserNameOrEmail(string search);
        Task<List<User>> GetUserByIds(List<Guid> ids);
    }
}
