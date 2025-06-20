using Microsoft.AspNetCore.SignalR;
using Presence.Services;
using System.Security.Claims;

namespace Presence.Hubs
{
    public class PresenceHub : Hub
    {
        private readonly IConnectionManager _connectionManager;
        private readonly ILogger<PresenceHub> _logger;

        public PresenceHub(IConnectionManager connectionManager, ILogger<PresenceHub> logger)
        {
            _connectionManager = connectionManager;
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _logger.LogInformation($"{Context}, {userId}");
            if (!string.IsNullOrEmpty(userId))
            {
                await _connectionManager.SetUserOnlineAsync(userId, Context.ConnectionId);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                await _connectionManager.SetUserOfflineAsync(userId);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }

}
