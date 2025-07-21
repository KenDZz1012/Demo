using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Channel.Domain.Common.Constants;
using MediatR;
using Service.Lib.BaseResponse;

namespace Channel.Application.Features.ServerMember.Commands.CreateServerMember
{
    public class CreateServerMember : IRequest<ApiResponse<Guid>>
    {
        public Guid ServerId { get; set; }

        public Guid UserId { get; set; }
        
        public string Role { get; set; } = ServerMemberRole.Member;
        public CreateServerMember() {} 

        public CreateServerMember(Guid serverId, Guid userId, string role)
        {
            ServerId = serverId;
            UserId = userId;
            Role = role;
        }
    }
}
