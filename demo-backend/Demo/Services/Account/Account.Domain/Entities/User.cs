using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Account.Domain.Common.Constants;

namespace Account.Domain.Entities
{
    public class User
    {
        public Guid ID { get; set; }

        public string UserName { get; set; }

        public string DisplayName { get; set; }

        public string PasswordHash { get; set; }

        public string Email { get; set; }

        public string? AvatarUrl { get; set; }

        public string Status { get; set; } = UserStatus.Pending;

        public DateTime DateOfBirth { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
