using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Service.Lib.BaseResponse;

namespace Account.Application.Features.User.Commands.DeleteUserCommand
{
    public class DeleteUser : IRequest<ApiResponse<Guid>>
    {
        public Guid ID { get; set; }
        public DeleteUser(Guid id)
        {
            ID = id;
        }
    }
}
