using Channel.Application.Features.Server.Commands.CreateServer;
using Channel.Application.Features.Server.Commands.UpdateServerIcon;
using Channel.Application.Features.Server.Queries.GetServers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Service.Lib.BaseResponse;

namespace Channel.API.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class ServerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ServerController(IMediator mediator)
        {
            _mediator = mediator;  
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<GetServersVm>>), StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetAllServer([FromQuery] GetServers filter)
        {
            var response = await _mediator.Send(filter);
            return response.IsSuccess ? Ok(response) : StatusCode(int.Parse(response.ErrorCode), response);
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateServer([FromBody] CreateServer server)
        {
            var response = await _mediator.Send(server);
            return response.IsSuccess ? Ok(response) : StatusCode(int.Parse(response.ErrorCode), response);
        }

        [HttpPost("UploadIcon")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UploadIcon([FromForm] UpdateServerIcon server)
        {
            var response = await _mediator.Send(server);
            return response.IsSuccess ? Ok(response) : StatusCode(int.Parse(response.ErrorCode), response);
        }
    }
}
