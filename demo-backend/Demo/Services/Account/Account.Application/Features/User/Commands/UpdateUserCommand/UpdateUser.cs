using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Service.Lib.BaseResponse;

namespace Account.Application.Features.User.Commands.UpdateUserCommand
{
    public class UpdateUser : IRequest<ApiResponse<Guid>>
    {
        public Guid ID { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }

        public UpdateUser(Guid id, string userName, string passwordHash, string email, string status)
        {
            ID = id;
            UserName = userName;
            PasswordHash = passwordHash;
            Email = email;
            Status = status;
        }
    }
}
