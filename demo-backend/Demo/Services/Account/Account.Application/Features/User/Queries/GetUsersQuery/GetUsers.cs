using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Service.Lib.BaseResponse;

namespace Account.Application.Features.User.Queries.GetUsersQuery
{
    public class GetUsers : IRequest<ApiResponse<List<GetUsersVm>>>
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Status { get; set; }
        public GetUsers() { }

        public GetUsers(string? userName, string? email, string? status)
        {
            UserName = userName;
            Email = email;
            Status = status;
        }
    }
}
