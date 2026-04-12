using Authorize.Application.Features.User.Commands.CreateIdentityUserCommand;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Service.Lib.BaseResponse;

namespace Authorize.Controllers;

[ApiController]
[Route("v1/")]
public class AuthorizeController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthorizeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("users")]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<Guid>>> CreateUser(
        [FromBody] CreateIdentityUser command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        if (!result.IsSuccess)
            return BadRequest(result);
        return Ok(result);
    }
}