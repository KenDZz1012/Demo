using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Service.Lib.BaseResponse;

namespace Channel.Application.Features.Server.Commands.DeleteServer
{
    public class DeleteServer : IRequest<ApiResponse<bool>>
    {
        public Guid Id { get; set; }
        
        public DeleteServer() { }

        public DeleteServer(Guid id)
        {
            Id = id;
        }
    }
}
