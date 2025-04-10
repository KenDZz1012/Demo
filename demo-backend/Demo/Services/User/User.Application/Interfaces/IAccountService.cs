using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Application.DTOs;

namespace User.Application.Interfaces
{
    public interface IAccountService
    {
        Task<List<AccountDTO>> GetAllAsync();
        Task<AccountDTO> GetByIdAsync(string id);
    }
}
