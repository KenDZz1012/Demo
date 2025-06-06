using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presence.Services;

namespace Presence.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PresenceController : ControllerBase
    {
        private readonly IConnectionManager _connectionManager;

        public PresenceController(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserStatus(string userId)
        {
            var status = await _connectionManager.GetUserStatusAsync(userId);
            return Ok(new { userId, status });
        }
    }

}
