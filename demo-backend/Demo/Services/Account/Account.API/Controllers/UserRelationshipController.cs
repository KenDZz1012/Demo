
using Account.Application.Features.User.Commands.CreateUserCommand;
using Account.Application.Features.User.Queries.GetUserByIDQuery;
using Account.Application.Features.User.Queries.GetUserQuery;
using Account.Application.Features.User.Queries.GetUsersQuery;
using Account.Application.Features.UserRelationship.Commands.CreateUserRelationshipCommand;
using Account.Application.Features.UserRelationship.Commands.DeleteUserRelationshipCommand;
using Account.Application.Features.UserRelationship.Commands.UpdateStatusCommand;
using Account.Application.Features.UserRelationship.Queries.GetListUserRelationshipQuery;
using Account.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Service.Lib.BaseResponse;

namespace Account.API.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class UserRelationshipController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserRelationshipController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<GetUsersVm>>), StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ApiResponse<List<GetListUserRelationshipVm>>> GetAllUserRelationship([FromQuery] GetListUserRelationship userRelationshipFilter)
        {
            var users = await _mediator.Send(userRelationshipFilter);
            return users;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        public async Task<ApiResponse<Guid>> AddUserRelationship([FromBody] CreateUserRelationship userRelationship)
        {
            return await _mediator.Send(userRelationship);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        public async Task<ApiResponse<Guid>> UpdateStatusRelationship([FromBody] UpdateStatus userRelationship)
        {
            return await _mediator.Send(userRelationship);
        }

        [HttpDelete]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        public async Task<ApiResponse<Guid>> DeleteUserRelationship([FromQuery] Guid ID)
        {
            return await _mediator.Send(new DeleteUserRelationship(ID));
        }
    }
}
