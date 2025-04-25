using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Account.Domain.Constants;
using Account.Domain.Model.User;

namespace Account.Application.DTOs.User
{
    public class UserCreateDTO : IUserCreateInfo
    {
        public string UserName { get; set; }

        public string PasswordHash { get; set; }

        public string Email { get; set; }

        public string Status { get; set; } = UserStatus.Pending;
    }
}
