using Authorize.Application.Features.Login.Commands.LoginCommand;
using Authorize.Application.Features.RefreshToken.Commands.RefreshTokenCommand;
using Authorize.Application.Models;
using Authorize.Model;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Service.Lib.BaseResponse;

namespace Authorize.Controllers
{
    [Route("v1/auth")]
    public class AuthorizeController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthorizeController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("login")]
        public async Task<ApiResponse<TokenResponse>> Authorization([FromBody] Login login)
        {
            return await _mediator.Send(login);
        }


        [HttpPost("refresh")]
        public async Task<ApiResponse<TokenResponse>> Refresh([FromBody] RefreshToken refreshToken)
        {
            return await _mediator.Send(refreshToken);
        }
    }
}
