using Authorize.Application.Features.Login.Commands.LoginCommand;
using Authorize.Application.Features.Logout.Commands.LogoutCommand;
using Authorize.Application.Features.RefreshToken.Commands.RefreshTokenCommand;
using Authorize.Application.Models;
using Authorize.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Service.Lib.BaseResponse;

namespace Authorize.Controllers
{
    [Route("v1/")]
    public class AuthorizeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthorizeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Authorization([FromBody] Login login)
        {
            var response = await _mediator.Send(login);
            return response.IsSuccess ? Ok(response) : StatusCode(int.Parse(response.ErrorCode), response);
        }


        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] Application.Features.RefreshToken.Commands.RefreshTokenCommand.RefreshToken refreshToken)
        {
            var response = await _mediator.Send(refreshToken);
            return response.IsSuccess ? Ok(response) : StatusCode(int.Parse(response.ErrorCode), response);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] Logout logout)
        {
            var response = await _mediator.Send(logout);
            return response.IsSuccess ? Ok(response) : StatusCode(int.Parse(response.ErrorCode), response);
        }
    }
}