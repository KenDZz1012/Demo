using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Domain.Entities
{
    public class Account
    {
        public string AccountID { get; set; }

        public string AccountName { get; set; }

        public bool isAdmin { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public bool isActive { get; set; }
    }
}
