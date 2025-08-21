using Channel.Application.Features.Server.Queries.GetServers;
using MediatR;
using Service.Lib.BaseResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Channel.Application.Features.Server.Queries.GetServerDetail
{
    public class GetServerDetail : IRequest<ApiResponse<GetServerDetailVm>>
    {
        public Guid Id { get; set; }

        public GetServerDetail() { }

        public GetServerDetail(Guid id)
        {
            Id = id;
        }
    }
}
