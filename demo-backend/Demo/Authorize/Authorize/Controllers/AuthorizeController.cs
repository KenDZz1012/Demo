using Authorize.Model;
using Authorize.Repositories;
using Microsoft.AspNetCore.Mvc;
using Service.Lib.BaseResponse;

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
        public async Task<ApiResponse<bool>> Authorization([FromBody] Login login)
        {
            var success = await _repository.Authorization(login, Response);
            return success;
        }


        [HttpPost("refresh")]
        public async Task<ApiResponse<bool>> Refresh()
        {
            var success = await _repository.RefreshTokenAsync(Request, Response);
            return success;
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _repository.LogoutAsync(Response);
            return Ok(new { message = "Logged out" });
        }
    }
}
