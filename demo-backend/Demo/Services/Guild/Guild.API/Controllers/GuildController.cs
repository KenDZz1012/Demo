using Guild.Application.Features.Guild.Commands.CreateGuild;
using Guild.Application.Features.Guild.Commands.DeleteGuild;
using Guild.Application.Features.Guild.Commands.UpdateGuild;
using Guild.Application.Features.Guild.Queries.GetGuild;
using Guild.Application.Features.Guild.Queries.GetGuilds;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Service.Lib.BaseResponse;

namespace Guild.API.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class GuildController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GuildController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("user/{userId}")]
        [ProducesResponseType(typeof(ApiResponse<List<GetGuildsVm>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetGuilds(Guid userId)
        {
            var response = await _mediator.Send(new GetGuilds(userId));
            return response.IsSuccess ? Ok(response) : StatusCode(int.Parse(response.ErrorCode), response);
        }

        [HttpGet("{guildId}")]
        [ProducesResponseType(typeof(ApiResponse<GetGuildVm>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetGuild(Guid guildId)
        {
            var response = await _mediator.Send(new GetGuild(guildId));
            return response.IsSuccess ? Ok(response) : StatusCode(int.Parse(response.ErrorCode), response);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateGuild([FromForm] CreateGuild command)
        {
            var response = await _mediator.Send(command);
            return response.IsSuccess ? Ok(response) : StatusCode(int.Parse(response.ErrorCode), response);
        }

        [HttpPut("{guildId}")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateGuild(Guid guildId, [FromForm] UpdateGuild command)
        {
            command.GuildId = guildId;
            var response = await _mediator.Send(command);
            return response.IsSuccess ? Ok(response) : StatusCode(int.Parse(response.ErrorCode), response);
        }

        [HttpDelete("{guildId}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteGuild(Guid guildId, [FromQuery] Guid deletedBy)
        {
            var response = await _mediator.Send(new DeleteGuild(guildId, deletedBy));
            return response.IsSuccess ? Ok(response) : StatusCode(int.Parse(response.ErrorCode), response);
        }
    }
}
