using MediatR;
using Service.Lib.BaseResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectMessage.Application.Features.DirectMessage.Commands.SendMessage
{
    public class SendMessage : IRequest<ApiResponse<string>>
    {
        public Guid senderId { get; set; }

        public List<Guid> recipientIds { get; set; }

        public string content { get; set; }

        public SendMessage() { }

        public SendMessage(Guid senderId, List<Guid> recipientIds, string content)
        {
            this.senderId = senderId;
            this.recipientIds = recipientIds;
            this.content = content;
        }
    }
}
