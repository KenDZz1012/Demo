using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Service.Lib.BaseResponse;

namespace Account.Application.Features.UserRelationship.Commands.CreateUserRelationshipCommand
{
    public class CreateUserRelationship : IRequest<ApiResponse<Guid>>
    {
        public Guid RequesterId { get; set; }
        public string AddresseeName { get; set; }
        public CreateUserRelationship(Guid requesterId, string addresseeName)
        {
            RequesterId = requesterId;
            AddresseeName = addresseeName;
        }
    }
}
