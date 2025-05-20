using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Service.Lib.BaseResponse;

namespace Account.Application.Features.User.Commands.CreateUserCommand
{
    public class CreateUser : IRequest<ApiResponse<Guid>>
    {
        public string? UserName { get; set; }

        public string? DisplayName { get; set; }

        public string? PasswordHash { get; set; }

        public string? Email { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public bool? IsAdmin { get; set; }
    }
}
