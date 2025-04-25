using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Application.DTOs.UserRelationship
{
    public class UserRelationshipDTO
    {
        public Guid ID { get; set; }

        public Guid RequesterId { get; set; }

        public string RequesterName { get; set; }

        public Guid AddresseeId { get; set; }

        public string AddresseeName { get; set; }

        public string Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
