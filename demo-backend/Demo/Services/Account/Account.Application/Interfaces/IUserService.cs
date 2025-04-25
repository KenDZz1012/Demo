using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Account.Application.DTOs.User;
using Account.Domain.Entities;
using Account.Domain.Filters;
using Account.Domain.Model.User;

namespace Account.Application.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDTO>> GetAllAsync(UserFilter userFilter);
        Task<UserDTO> GetByIdAsync(Guid id);
        Task AddAsync(UserCreateDTO user);
        Task UpdateAsync(User user);
        Task DeleteAsync(Guid id);
    }
}
