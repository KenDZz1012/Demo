using Authorize.Model;
using Authorize.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Authorize.Controllers
{
    [Route("v1/")]
    public class AuthorizeController : ControllerBase
    {
        private readonly IAuthorizeRepository _repository;
        public AuthorizeController(IAuthorizeRepository repository)
        {
            _repository = repository;
        }
        [HttpPost("Authorization")]
        public async Task<IActionResult> Authorization([FromBody] Login login)
        {
            var user = await _repository.Authorization(login);
            return Ok(user);
        }
    }
}
