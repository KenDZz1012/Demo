using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Service.Lib.BaseResponse;

namespace Account.Application.Features.UserRelationship.Commands.CreateUserRelationshipCommand
{
    public class CreateUserRelationship : IRequest<ApiResponse<CreateUserRelationshipResponse>>
    {
        public Guid RequesterId { get; set; }
        public string AddresseeName { get; set; }
        public CreateUserRelationship(Guid requesterId, string addresseeName)
        {
            RequesterId = requesterId;
            AddresseeName = addresseeName;
        }
    }

    public class CreateUserRelationshipResponse
    {
        public Guid ID { get; set; }

        public string UserName { get; set; }

        public string DisplayName { get; set; }
        
        public string AvatarUrl { get; set; }
    }
}
