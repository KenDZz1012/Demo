using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Presence.Services;
using System.Security.Claims;
using System.Text.Json;

namespace Presence.Hubs
{
    [Authorize]
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
            if (Context.User?.Identity?.IsAuthenticated == true)
            {
                Console.WriteLine("User is authenticated");
                foreach (var claim in Context.User.Claims)
                {
                    Console.WriteLine($"Claim: {claim.Type} = {claim.Value}");
                }
            }
            else
            {
                Console.WriteLine("User is NOT authenticated");
            }

            // Try different claim types for Keycloak
            var userId = Context.User?.FindFirstValue("sub") ??           
                         Context.User?.FindFirstValue("preferred_username") ?? 
                         Context.User?.FindFirstValue("email") ??              
                         Context.User?.FindFirstValue("user_id") ??           
                         Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);

            Console.WriteLine($"Final userId: {userId}");

            if (!string.IsNullOrEmpty(userId))
            {
                await _connectionManager.SetUserOnlineAsync(userId, Context.ConnectionId);
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User?.FindFirstValue("sub") ?? Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!string.IsNullOrEmpty(userId))
            {
                Console.WriteLine($"User disconnected: {userId}, connection: {Context.ConnectionId}");
                await _connectionManager.SetUserOfflineAsync(userId, Context.ConnectionId);
            }

            await base.OnDisconnectedAsync(exception);
        }

        
        public async Task Heartbeat()
        {
            var userId = Context.User?.FindFirstValue("sub") ??           
                         Context.User?.FindFirstValue("preferred_username") ?? 
                         Context.User?.FindFirstValue("email") ??              
                         Context.User?.FindFirstValue("user_id") ??           
                         Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (!string.IsNullOrEmpty(userId))
            {
                Console.WriteLine($"❤️ Received heartbeat from user: {userId}");
                await _connectionManager.SetUserOnlineAsync(userId, Context.ConnectionId);
            }
        }
        
        public async Task<Dictionary<string, bool>> GetFriendsStatus(List<string> userIds)
        {
            return await _connectionManager.GetBatchStatus(userIds);
        }
        
        public async Task SendFriendRequestNotification(string toUserId, string fromUserId)
        {
            var connections = await _connectionManager.GetConnectionIdsAsync(toUserId);
            foreach (var connectionId in connections)
            {
                await Clients.Client(connectionId).SendAsync("friendRequestReceived", new { fromUserId });
            }
        }

    }

}
