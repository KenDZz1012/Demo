using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Account.Domain.Common.Constants;

namespace Account.Domain.Entities
{
    public partial class UserRelationship
    {
        public Guid Id { get; set; }

        public Guid RequesterId { get; set; }

        public Guid AddresseeId { get; set; }

        public string Status { get; set; } = null!;

        public DateTime? CreatedAt { get; set; }

        public virtual User Addressee { get; set; } = null!;

        public virtual User Requester { get; set; } = null!;
    }

}
