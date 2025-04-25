using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Service.Lib.BaseResponse;

namespace Account.Application.Features.User.Queries.GetUsersQuery
{
    public class GetUsersQuery : IRequest<ApiResponse<GetUsersQueryVm>>
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Status { get; set; }
    }
}
