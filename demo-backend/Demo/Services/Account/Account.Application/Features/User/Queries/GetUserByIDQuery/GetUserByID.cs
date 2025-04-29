using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Account.Application.Features.User.Queries.GetUserByIDQuery;
using MediatR;
using Service.Lib.BaseResponse;

namespace Account.Application.Features.User.Queries.GetUserQuery
{
    public class GetUserByID : IRequest<ApiResponse<GetUserByIDVm>>
    {
        public Guid ID { get; set; }

        public GetUserByID(Guid id)
        {
            ID = id;
        }
    }
}
