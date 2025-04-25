using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Account.Domain.Common.Constants;

namespace Account.Domain.Entities
{
    public class UserRelationship
    {
        public Guid ID { get; set; }

        public Guid RequesterId { get; set; }

        public Guid AddresseeId { get; set; }

        public string Status { get; set; } = UserRelationshipStatus.Pending;

        public DateTime CreatedAt { get; set; }

        public virtual User Requester { get; set; }

        public virtual User Addressee { get; set; }
    }
}
