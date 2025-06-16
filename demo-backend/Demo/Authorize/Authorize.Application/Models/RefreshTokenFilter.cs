using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorize.Application.Models
{
    public class RefreshTokenFilter
    {
        public string? RefreshToken { get; set; }

        public Guid? UserId { get; set; }
    }
}
