using MediatR;
using Microsoft.AspNetCore.Http;
using Service.Lib.BaseResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Channel.Application.Features.Server.Commands.CreateServer
{
    public class CreateServer : IRequest<ApiResponse<Guid>>
    {
        public string Name { get; set; }

        public Guid OwnerId { get; set; }

        public IFormFile File { get; set; }

        public CreateServer(string name, Guid ownerId, IFormFile file)
        {
            Name = name;
            OwnerId = ownerId;
            File = file;
        }
    }
}
