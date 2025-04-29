using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Service.Lib.BaseResponse;

namespace Account.Application.Features.User.Commands.UpdateUserNameCommand
{
    public class UpdateUserName : IRequest<ApiResponse<Guid>>
    {
        public Guid ID { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }

        public UpdateUserName(Guid id, string userName, string passwordHash)
        {
            ID = id;
            UserName = userName;
            PasswordHash = passwordHash;
        }
    }
}
