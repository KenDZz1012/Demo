using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Channel.Application.Features.Server.Queries.GetServer
{
    public class GetServerDetailVm
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string OwnerId { get; set; }

        public string? IconUrl { get; set; }

        public List<Channel> Channels { get; set; } = new List<Channel>();

        public List<ServerMember> ServerMembers { get; set; } = new List<ServerMember>();
    }

    public class Channel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }

    public class ServerMember
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string AvatarUrl { get; set; }
        public string Role { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
    }
}
