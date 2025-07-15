using MediatR;
using Microsoft.AspNetCore.Http;
using Service.Lib.BaseResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Channel.Application.Features.Server.Queries.GetServers;

namespace Channel.Application.Features.Server.Commands.CreateServer
{
    public class CreateServer : IRequest<ApiResponse<Guid>>
    {
        public string Name { get; set; }

        public Guid OwnerId { get; set; }

        public string? IconUrl { get; set; }

        public CreateServer() {}

        public CreateServer(string name, Guid ownerId, string? iconUrl)
        {
            Name = name;
            OwnerId = ownerId;
            IconUrl = iconUrl;
        }
    }
}
