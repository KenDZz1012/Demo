using Guild.Application.Features.GuildMember.Commands.JoinGuild;
using Guild.Application.Features.GuildMember.Commands.KickMember;
using Guild.Application.Features.GuildMember.Commands.LeaveGuild;
using Guild.Application.Features.GuildMember.Queries.GetGuildMembers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Lib.BaseResponse;

namespace Guild.API.Controllers
{
    [ApiController]
    [Route("v1/guild")]
    public class GuildMemberController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GuildMemberController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{guildId}/members")]
        [Authorize(Policy = "Guild.Read")]
        [ProducesResponseType(typeof(ApiResponse<List<GetGuildMembersVm>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMembers(Guid guildId)
        {
            var response = await _mediator.Send(new GetGuildMembers(guildId));
            return response.IsSuccess ? Ok(response) : StatusCode(int.Parse(response.ErrorCode), response);
        }

        [HttpPost("join")]
        [Authorize(Policy = "Guild.Read")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> JoinGuild([FromBody] JoinGuild command)
        {
            var response = await _mediator.Send(command);
            return response.IsSuccess ? Ok(response) : StatusCode(int.Parse(response.ErrorCode), response);
        }

        [HttpPost("{guildId}/leave")]
        [Authorize(Policy = "Guild.Read")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> LeaveGuild(Guid guildId, [FromQuery] Guid userId)
        {
            var response = await _mediator.Send(new LeaveGuild { GuildId = guildId, UserId = userId });
            return response.IsSuccess ? Ok(response) : StatusCode(int.Parse(response.ErrorCode), response);
        }

        [HttpDelete("{guildId}/members/{targetUserId}")]
        [Authorize(Policy = "Guild.Manage")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> KickMember(Guid guildId, Guid targetUserId, [FromQuery] Guid kickedBy)
        {
            var response = await _mediator.Send(new KickMember
            {
                GuildId = guildId,
                TargetUserId = targetUserId,
                KickedBy = kickedBy
            });
            return response.IsSuccess ? Ok(response) : StatusCode(int.Parse(response.ErrorCode), response);
        }
    }
}
