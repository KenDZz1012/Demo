using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Lib.BaseResponse;

namespace Account.Application.Features.User.Queries.GetUsersQuery
{
    public class GetUsersQueryVm
    {
        public Guid? ID { get; set; }

        public string? UserName { get; set; }

        public string? Email { get; set; }

        public string? AvatarUrl { get; set; }
    }
}
