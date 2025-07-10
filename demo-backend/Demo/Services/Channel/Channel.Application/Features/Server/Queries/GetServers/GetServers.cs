using Channel.Application.Features.ServerMember.Queries.GetServerMembers;
using MediatR;
using Service.Lib.BaseResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Channel.Application.Features.Server.Queries.GetServers
{
    public class GetServers : IRequest<ApiResponse<List<GetServersVm>>>
    {
        public Guid? OwnerId { get; set; }

        public GetServers() {} 
        
        public GetServers(Guid? ownerId)
        {
            OwnerId = ownerId;
        }
    }
}
