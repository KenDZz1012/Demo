using Channel.Application.Features.Server.Queries.GetServers;
using MediatR;
using Service.Lib.BaseResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Channel.Application.Features.Server.Queries.GetServer
{
    public class GetServer : IRequest<ApiResponse<GetServerVm>>
    {
        public Guid Id { get; set; }

        public GetServer() { }

        public GetServer(Guid id)
        {
            Id = id;
        }
    }
}
