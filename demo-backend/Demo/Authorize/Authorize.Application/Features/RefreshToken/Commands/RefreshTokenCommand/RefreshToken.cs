using Authorize.Application.Models;
using MediatR;
using Service.Lib.BaseResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorize.Application.Features.RefreshToken.Commands.RefreshTokenCommand
{
    public class RefreshToken : IRequest<ApiResponse<RefreshTokenResponse>>
    {
        public string refreshToken { get; set; }

        public Guid userID { get; set; }
    }
}
