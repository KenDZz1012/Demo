using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Service.Lib.BaseResponse;

namespace Account.Application.Features.UserRelationship.Commands.UpdateStatusCommand
{
    public class UpdateStatus : IRequest<ApiResponse<Guid>>
    {
        public Guid ID { get; set; }

        public string Status { get; set; }

        public UpdateStatus(Guid id, string status)
        {
            ID = id;
            Status = status;
        }
    }
}
