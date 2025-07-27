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
        public Guid UserID { get; set; }

        public Guid FriendID { get; set; }

        public string Status { get; set; }

        public UpdateStatus() {  }

        public UpdateStatus(Guid userID, Guid friendID, string status)
        {
            UserID = userID;
            FriendID = friendID;
            Status = status;
        }
    }
}
