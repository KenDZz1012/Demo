using Microsoft.AspNetCore.Mvc;
using MediatR;
using Account.Application.Features.User.Queries.GetUsersQuery;
using Service.Lib.BaseResponse;
using Account.Application.Features.User.Queries.GetUserByIDQuery;
using Account.Application.Features.User.Queries.GetUserQuery;
using Account.Application.Features.User.Commands.CreateUserCommand;
using Account.Application.Features.User.Commands.UpdateUserCommand;
using Account.Application.Features.User.Commands.DeleteUserCommand;
using Account.Application.Features.User.Commands.UpdateUserNameCommand;
using Account.Application.Features.User.Commands.UpdateEmailCommand;
using Account.Application.Features.User.Commands.UpdateDisplayNameCommand;
using Account.Application.Features.User.Commands.UpdateAvatarCommand;

namespace Account.API.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<GetUsersVm>>), StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetAllUsers([FromQuery] GetUsers userFilter)
        {
            var response = await _mediator.Send(userFilter);
            return response.IsSuccess ? Ok(response) : StatusCode(int.Parse(response.ErrorCode), response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<GetUserByIDVm>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var response = await _mediator.Send(new GetUserByID(id));
            return response.IsSuccess ? Ok(response) : StatusCode(int.Parse(response.ErrorCode), response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        public async Task<IActionResult> AddUser([FromBody] CreateUser user)
        {
            var response = await _mediator.Send(user);
            return response.IsSuccess ? Ok(response) : StatusCode(int.Parse(response.ErrorCode), response);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUser user)
        {
            var response = await _mediator.Send(user);
            return response.IsSuccess ? Ok(response) : StatusCode(int.Parse(response.ErrorCode), response);
        }

        [HttpDelete]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteUser([FromQuery] Guid id)
        {
            var response = await _mediator.Send(new DeleteUser(id));
            return response.IsSuccess ? Ok(response) : StatusCode(int.Parse(response.ErrorCode), response);
        }

        [HttpPut("UpdateUserName")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateUserName([FromBody] UpdateUserName user)
        {
            var response = await _mediator.Send(user);
            return response.IsSuccess ? Ok(response) : StatusCode(int.Parse(response.ErrorCode), response);
        }

        [HttpPut("UpdateUserEmail")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateUserEmail([FromBody] UpdateEmail user)
        {
            var response = await _mediator.Send(user);
            return response.IsSuccess ? Ok(response) : StatusCode(int.Parse(response.ErrorCode), response);
        }

        [HttpPut("UpdateUserDisplayName")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateUserDisplayName([FromBody] UpdateDisplayName user)
        {
            var response = await _mediator.Send(user);
            return response.IsSuccess ? Ok(response) : StatusCode(int.Parse(response.ErrorCode), response);
        }

        [HttpPut("UpdateUserAvatar")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateUserAvatar([FromForm] IFormFile file, [FromForm] Guid ID)
        {
            var response = await _mediator.Send(new UpdateAvatar(ID, file));
            return response.IsSuccess ? Ok(response) : StatusCode(int.Parse(response.ErrorCode), response);
        }
    }
}