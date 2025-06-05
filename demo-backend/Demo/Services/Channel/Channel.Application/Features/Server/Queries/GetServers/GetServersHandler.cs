using MediatR;
using Service.Lib.BaseResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Channel.Application.Features.Server.Queries.GetServers
{
    public class GetServersHandler : IRequestHandler<GetServers, ApiResponse<List<GetServersVm>>>
    {
        public Task<ApiResponse<List<GetServersVm>>> Handle(GetServers request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
