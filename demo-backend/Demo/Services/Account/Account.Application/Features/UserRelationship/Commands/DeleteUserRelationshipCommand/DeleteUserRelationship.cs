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
        public Guid ID { get; set; }
        public DeleteUserRelationship(Guid id)
        {
            ID = id;
        }
    }
}
