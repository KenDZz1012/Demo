using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Account.Application.Interfaces;
using Account.Domain.Entities;
using Account.Domain.Filters;
using Account.Domain.Model.User;
using Account.Application.DTOs.User;

namespace Account.API.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllUsers([FromQuery] UserFilter userFilter)
        {
            var users = await _userService.GetAllAsync(userFilter);
            return Ok(users);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _userService.GetByIdAsync(id);
            return user != null ? Ok(user) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] UserCreateDTO user)
        {
            if (user == null)
            {
                return BadRequest("User cannot be null");
            }
            await _userService.AddAsync(user);
            return Ok("Thêm thành công");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("User cannot be null");
            }
            await _userService.UpdateAsync(user);
            return Ok("Cập nhật thành công");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            await _userService.DeleteAsync(id);
            return Ok("Xóa thành công");
        }
    }
}
