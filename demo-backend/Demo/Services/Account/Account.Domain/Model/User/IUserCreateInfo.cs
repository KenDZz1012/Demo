using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Domain.Model.User
{
    public interface IUserCreateInfo
    {
        string UserName { get; set; }
        string Email { get; set; }
        string PasswordHash { get; set; }
        string Status { get; set; } 
    }
}
