using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Account.Domain.Entities;
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
using Account.Application.Features.User.Commands.UpdatePasswordCommand;

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
        public async Task<ApiResponse<List<GetUsersVm>>> GetAllUsers([FromQuery] GetUsers userFilter)
        {
            var users = await _mediator.Send(userFilter);
            return users;
        }
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<GetUserByIDVm>), StatusCodes.Status200OK)]
        public async Task<ApiResponse<GetUserByIDVm>> GetUserById(Guid id)
        {
            var user = await _mediator.Send(new GetUserByID(id));
            return user;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        public async Task<ApiResponse<Guid>> AddUser([FromBody] CreateUser user)
        {
            return await _mediator.Send(user);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        public async Task<ApiResponse<Guid>> UpdateUser([FromBody] UpdateUser user)
        {
            return await _mediator.Send(user);
        }

        [HttpDelete]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        public async Task<ApiResponse<Guid>> DeleteUser([FromQuery] Guid id)
        {
            return await _mediator.Send(new DeleteUser(id));
        }

        [HttpPut("UpdateUserName")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        public async Task<ApiResponse<Guid>> UpdateUserName([FromBody] UpdateUserName user)
        {
            return await _mediator.Send(user);
        }

        [HttpPut("UpdateUserEmail")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        public async Task<ApiResponse<Guid>> UpdateUserEmail([FromBody] UpdateEmail user)
        {
            return await _mediator.Send(user);
        }

        [HttpPut("UpdateUserDisplayName")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        public async Task<ApiResponse<Guid>> UpdateUserDisplayName([FromBody] UpdateDisplayName user)
        {
            return await _mediator.Send(user);
        }

        [HttpPut("UpdateUserAvatar")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        public async Task<ApiResponse<Guid>> UpdateUserAvatar([FromForm] IFormFile file, [FromForm] Guid ID)
        {
            return await _mediator.Send(new UpdateAvatar(ID, file));
        }

        [HttpPut("UpdateUserPassword")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        public async Task<ApiResponse<Guid>> UpdateUserPassword([FromBody] UpdatePassword user)
        {
            return await _mediator.Send(user);
        }
    }
}
