using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Channel.Domain.Entities;

namespace Channel.Application.Contracts.Persistence
{
    public interface IServerMemberRepository
    {
        Task<bool> AddAsync(ServerMember serverMember);

        Task<ServerMember> CheckUserExistInServer(Guid serverId, Guid userId);
    }
}
