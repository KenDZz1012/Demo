using Account.Application.Features.User.Queries.GetUsersQuery;
using Account.Application.Features.UserRelationship.Commands.CreateUserRelationshipCommand;
using Account.Application.Features.UserRelationship.Commands.DeleteUserRelationshipCommand;
using Account.Application.Features.UserRelationship.Commands.UpdateStatusCommand;
using Account.Application.Features.UserRelationship.Queries.GetListFriendQuery;
using Account.Application.Features.UserRelationship.Queries.GetListUserRelationshipQuery;
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
        public async Task<IActionResult> GetAllUserRelationship(
            [FromQuery] GetListUserRelationship userRelationshipFilter)
        {
            var response = await _mediator.Send(userRelationshipFilter);
            return response.IsSuccess ? Ok(response) : StatusCode(int.Parse(response.ErrorCode), response);
        }
        
        [HttpGet("Friends")]
        [ProducesResponseType(typeof(ApiResponse<List<GetListFriendVm>>), StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> GetListFriends(
            [FromQuery] GetListFriend userRelationshipFilter)
        {
            var response = await _mediator.Send(userRelationshipFilter);
            return response.IsSuccess ? Ok(response) : StatusCode(int.Parse(response.ErrorCode), response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        public async Task<IActionResult> AddUserRelationship([FromBody] CreateUserRelationship userRelationship)
        {
            var response = await _mediator.Send(userRelationship);
            return response.IsSuccess ? Ok(response) : StatusCode(int.Parse(response.ErrorCode), response);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateStatusRelationship([FromBody] UpdateStatus userRelationship)
        {
            var response = await _mediator.Send(userRelationship);
            return response.IsSuccess ? Ok(response) : StatusCode(int.Parse(response.ErrorCode), response);
        }

        [HttpDelete]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteUserRelationship([FromQuery] Guid ID)
        {
            var response = await _mediator.Send(new DeleteUserRelationship(ID));  
            return response.IsSuccess ? Ok(response) : StatusCode(int.Parse(response.ErrorCode), response);
        }
    }
}