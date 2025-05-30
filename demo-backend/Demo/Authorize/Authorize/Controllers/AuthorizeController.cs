using Authorize.Model;
using Authorize.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Authorize.Controllers
{
    [Route("v1/auth")]
    public class AuthorizeController : ControllerBase
    {
        private readonly IAuthorizeRepository _repository;
        public AuthorizeController(IAuthorizeRepository repository)
        {
            _repository = repository;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Authorization([FromBody] Login login)
        {
            var success = await _repository.Authorization(login, Response);
            if (!success)
            {
                return Unauthorized(new { message = "Invalid username or password." });
            }

            return Ok(new { message = "Login successful" });
        }


        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            var success = await _repository.RefreshTokenAsync(Request, Response);
            return success ? Ok() : Unauthorized();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _repository.LogoutAsync(Response);
            return Ok(new { message = "Logged out" });
        }
    }
}
