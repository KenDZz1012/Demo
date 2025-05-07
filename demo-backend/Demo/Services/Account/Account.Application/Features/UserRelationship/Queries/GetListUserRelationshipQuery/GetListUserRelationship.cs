using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Service.Lib.BaseResponse;

namespace Account.Application.Features.UserRelationship.Queries.GetListUserRelationshipQuery
{
    public class GetListUserRelationship : IRequest<ApiResponse<List<GetListUserRelationshipVm>>>
    {
        public Guid? RequesterId { get; set; }

        public Guid? AddresseeId { get; set; }

        public string? Status { get; set; }

        public GetListUserRelationship() { }

        public GetListUserRelationship(Guid? requesterId, string? status, Guid? addresseeId)
        {
            RequesterId = requesterId;
            Status = status;
            AddresseeId = addresseeId;
        }
    }
}
