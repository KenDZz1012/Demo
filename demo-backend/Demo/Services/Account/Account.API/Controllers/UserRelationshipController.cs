using Account.Application.Interfaces;
using Account.Application.Services;
using Account.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Account.API.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class UserRelationshipController : ControllerBase
    {
        public readonly IUserRelationshipService _userRelationshipService;

        public UserRelationshipController(IUserRelationshipService userRelationshipService)
        {
            _userRelationshipService = userRelationshipService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUserRelationships()
        {
            var userRelationships = await _userRelationshipService.GetAllAsync();
            return Ok(userRelationships);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserRelationShipById(Guid id)
        {
            var userRelationship = await _userRelationshipService.GetByIdAsync(id);
            return userRelationship != null ? Ok(userRelationship) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AddUserRelationship([FromBody] UserRelationship userRelationship)
        {
            if (userRelationship == null)
            {
                return BadRequest("User relationship cannot be null");
            }
            await _userRelationshipService.AddAsync(userRelationship);
            return CreatedAtAction(nameof(GetUserRelationShipById), new { ID = userRelationship.ID }, userRelationship);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserRelationship([FromBody] UserRelationship userRelationship)
        {
            if (userRelationship == null)
            {
                return BadRequest("User relationship cannot be null");
            }
            await _userRelationshipService.UpdateAsync(userRelationship);
            return Ok("Cập nhật thành công");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            await _userRelationshipService.DeleteAsync(id);
            return Ok("Xóa thành công");
        }
    }
}
