using Channel.Application.Features.ServerMember.Commands.CreateServerMember;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Channel.API.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class ServerMemberController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ServerMemberController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateServer([FromBody] CreateServerMember serverMember)
        {
            var response = await _mediator.Send(serverMember);
            return response.IsSuccess ? Ok(response) : StatusCode(int.Parse(response.ErrorCode), response);
        }
    }
}
