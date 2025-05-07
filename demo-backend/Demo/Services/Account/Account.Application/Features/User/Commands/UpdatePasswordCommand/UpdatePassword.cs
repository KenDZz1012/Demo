using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Service.Lib.BaseResponse;

namespace Account.Application.Features.User.Commands.UpdatePasswordCommand
{
    public class UpdatePassword : IRequest<ApiResponse<Guid>>
    {
        public Guid ID { get; set; }
        public string PasswordHash { get; set; }

        public string NewPasswordHash { get; set; }

        public UpdatePassword(Guid id, string passwordHash, string newPasswordHash)
        {
            ID = id;
            PasswordHash = passwordHash;
            NewPasswordHash = newPasswordHash;
        }
    }
}
