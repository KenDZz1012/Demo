using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Application.Features.User.Queries.GetUserByIDQuery
{
    public class GetUserByIDVm
    {
        public Guid ID { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string AvatarUrl { get; set; }
    }
}
