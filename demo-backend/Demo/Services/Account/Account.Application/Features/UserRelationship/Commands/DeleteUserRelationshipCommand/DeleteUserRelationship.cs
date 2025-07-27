using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Service.Lib.BaseResponse;

namespace Account.Application.Features.UserRelationship.Commands.DeleteUserRelationshipCommand
{
    public class DeleteUserRelationship : IRequest<ApiResponse<Guid>>
    {
        public Guid UserID { get; set; }

        public Guid FriendID { get; set; }

        public DeleteUserRelationship() {  }

        public DeleteUserRelationship(Guid userID, Guid friendID)
        {
            UserID = friendID;
            FriendID = friendID;
        }
    }
}
