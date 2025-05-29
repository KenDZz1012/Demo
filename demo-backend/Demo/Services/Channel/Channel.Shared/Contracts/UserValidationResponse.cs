using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Channel.Shared.Contracts
{
    public class UserValidationResponse
    {
        public string CorrelationId { get; set; }
        public Guid UserId { get; set; }
        public bool Exists { get; set; }
    }
}
