using Channel.Application.Features.Channel.Commands.CreateChannel;
using Channel.Application.Features.Channel.Commands.DeleteChannel;
using Channel.Application.Features.Channel.Queries.GetChannels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service.Lib.BaseResponse;

namespace Channel.API.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class ChannelController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ChannelController> _logger;

        public ChannelController(IMediator mediator, ILogger<ChannelController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("guild/{guildId}")]
        [Authorize(Policy = "Channel.Read")]
        [ProducesResponseType(typeof(ApiResponse<List<GetChannelsVm>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetChannels(Guid guildId)
        {
            _logger.LogInformation("HTTP GET channels requested for GuildId: {GuildId}", guildId);
            var response = await _mediator.Send(new GetChannels(guildId));
            return response.IsSuccess ? Ok(response) : StatusCode(int.Parse(response.ErrorCode), response);
        }

        [HttpPost]
        [Authorize(Policy = "Channel.Create")]
        [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateChannel([FromBody] CreateChannel channel)
        {
            var response = await _mediator.Send(channel);
            return response.IsSuccess ? Ok(response) : StatusCode(int.Parse(response.ErrorCode), response);
        }

        [HttpDelete("{channelId}")]
        [Authorize(Policy = "Channel.Delete")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteChannel(Guid channelId, [FromQuery] Guid deletedBy)
        {
            var response = await _mediator.Send(new DeleteChannel(channelId, deletedBy));
            return response.IsSuccess ? Ok(response) : StatusCode(int.Parse(response.ErrorCode), response);
        }
    }
}
