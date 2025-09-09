using DirectMessage.Application.Features.DirectMessage.Commands.SendMessage;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Service.Lib.BaseResponse;

namespace DirectMessage.API.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class DirectMessageController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DirectMessageController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("SendMessage")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> SendMessage([FromBody] SendMessage sendMessage)
        {
            var response = await _mediator.Send(sendMessage);
            return response.IsSuccess ? Ok(response) : StatusCode(int.Parse(response.ErrorCode), response);
        }
    }
}
