using Authorize.Application.Models;
using MediatR;
using Service.Lib.BaseResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorize.Application.Features.Logout.Commands.LogoutCommand
{
    public class Logout : IRequest<ApiResponse<bool>>
    {
        public string RefreshToken { get; set; }

        public Logout() { }

        public Logout(string refreshToken)
        {
            RefreshToken = refreshToken;
        }
    }
}
