using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Domain.Filters
{
    public class UserRelationshipFilter
    {
        public string? RequesterName { get; set; }

        public string? AddresseeName { get; set; }   

        public string? Status { get; set; }
    }
}
