using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Channel.Domain.Common.Constants;
using MediatR;
using Service.Lib.BaseResponse;

namespace Channel.Application.Features.Channel.Commands.CreateChannel
{
    public class CreateChannel : IRequest<ApiResponse<Guid>>
    {
        public string Name { get; set; }

        public Guid ServerId { get; set; }

        public string Type { get; set; } = ChannelType.Text;

        public CreateChannel()
        {
        } 

        public CreateChannel(string name, Guid serverId, string type)
        {
            Name = name;
            ServerId = serverId;
            Type = type;
        }
    }
}
