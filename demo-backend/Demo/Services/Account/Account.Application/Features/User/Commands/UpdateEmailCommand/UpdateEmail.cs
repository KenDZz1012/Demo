using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Service.Lib.BaseResponse;

namespace Account.Application.Features.User.Commands.UpdateEmailCommand
{
    public class UpdateEmail : IRequest<ApiResponse<Guid>>
    {
        public Guid ID { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public UpdateEmail(Guid id, string email, string passwordHash)
        {
            ID = id;
            Email = email;
            PasswordHash = passwordHash;
        }
    }
}
