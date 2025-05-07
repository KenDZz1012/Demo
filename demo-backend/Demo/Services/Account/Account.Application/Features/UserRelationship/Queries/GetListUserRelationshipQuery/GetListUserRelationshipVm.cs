using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Account.Application.Features.User.Queries.GetUsersQuery;

namespace Account.Application.Features.UserRelationship.Queries.GetListUserRelationshipQuery
{
    public class GetListUserRelationshipVm
    {
        public Guid ID { get; set; }
        public Guid RequesterId { get; set; }
        public string RequesterName { get; set; }
        public Guid AddresseeId { get; set; }
        public string AddresseeName { get; set; }
        public string Status { get; set; }
    }
}
