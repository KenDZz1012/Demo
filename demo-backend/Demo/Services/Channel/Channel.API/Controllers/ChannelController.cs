using Channel.Application.Features.Channel.Commands.CreateChannel;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Channel.API.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class ChannelController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ChannelController(IMediator mediator)
        {
            _mediator = mediator;  
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateChannel([FromBody] CreateChannel channel)
        {
            var response = await _mediator.Send(channel);
            return response.IsSuccess ? Ok(response) : StatusCode(int.Parse(response.ErrorCode), response);
        }
    }
}
