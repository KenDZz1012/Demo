using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Presence.Hubs;
using Presence.Models;
using Presence.Services;

namespace Presence.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class PresenceController : ControllerBase
    {
        private readonly IConnectionManager _connectionManager;
        private readonly IHubContext<PresenceHub> _hubContext;

        public PresenceController(IConnectionManager connectionManager, IHubContext<PresenceHub> hubContext)
        {
            _connectionManager = connectionManager;
            _hubContext = hubContext;
        }


        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserStatus(string userId)
        {
            var status = await _connectionManager.GetUserStatusAsync(userId);
            return Ok(new { userId, status });
        }

        [HttpPost("batch-status")]
        public async Task<IActionResult> GetBatchStatus([FromBody] List<string> userIds)
        {
            return Ok(await _connectionManager.GetBatchStatus(userIds));
        }

        [HttpPost("friend-request")]
        public async Task<IActionResult> FriendRequest([FromBody] FriendRequestPayload payload)
        {
            var connections = await _connectionManager.GetConnectionIdsAsync(payload.ToUserName);
            foreach (var connId in connections)
            {
                await _hubContext.Clients.Client(connId).SendAsync("friendRequestReceived",
                    new
                    {
                        fromUserId = payload.FromUserId, fromUserName = payload.FromUserName,
                        fromUserDisplayName = payload.FromUserDisplayName, fromUserAvatarUrl = payload.FromUserAvatarUrl
                    });
            }

            return Ok();
        }
    }
}