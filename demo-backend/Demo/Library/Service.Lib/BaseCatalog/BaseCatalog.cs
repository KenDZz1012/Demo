using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Lib.BaseCatalog
{
    public class BaseCatalog
    {
        public string? UserCreated { get; set; }

        public string? UserModified { get; set; }

        public DateTime? CreatedTime { get; set; }

        public DateTime? ModifiedTime { get; set; }
    }
}
