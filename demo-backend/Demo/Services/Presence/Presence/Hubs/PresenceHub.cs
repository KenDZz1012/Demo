using Microsoft.AspNetCore.SignalR;
using Presence.Services;
using System.Security.Claims;

namespace Presence.Hubs
{
    public class PresenceHub : Hub
    {
        private readonly IConnectionManager _connectionManager;

        public PresenceHub(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
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
