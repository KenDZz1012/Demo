using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Channel.Application.Features.Server.Queries.GetServers
{
    public class GetServersVm
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string OwnerId { get; set; }

        public string? IconUrl { get; set; }
    }
}