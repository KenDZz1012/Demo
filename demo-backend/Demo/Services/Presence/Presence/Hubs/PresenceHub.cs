using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Presence.Services;
using System.Security.Claims;
using System.Text.Json;
using Account.Grpc.Protos;
using Presence.GrpcService;

namespace Presence.Hubs
{
    [Authorize]
    public class PresenceHub : Hub
    {
        private readonly IConnectionManager _connectionManager;
        private readonly ILogger<PresenceHub> _logger;
        private readonly UserGrpcService _userGrpcService;

        public PresenceHub(IConnectionManager connectionManager, ILogger<PresenceHub> logger,
            UserGrpcService userGrpcService)
        {
            _connectionManager = connectionManager;
            _logger = logger;
            _userGrpcService = userGrpcService;
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

            var userId = Context.User?.FindFirstValue("sub") ??
                         Context.User?.FindFirstValue("preferred_username") ??
                         Context.User?.FindFirstValue("email") ??
                         Context.User?.FindFirstValue("user_id") ??
                         Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!string.IsNullOrEmpty(userId))
            {
                await _connectionManager.SetUserOnlineAsync(userId, Context.ConnectionId);
            }

            var friends = await _userGrpcService.GetListFriend(new GetListFriendRequest() { UserId = userId });
            foreach (var friend in friends.Friends)
            {
                var connections = await _connectionManager.GetConnectionIdsAsync(friend.UserName);
                foreach (var connectionId in connections)
                {
                    await Clients.Client(connectionId)
                        .SendAsync("friendStatusChanged", new { userName = userId, isOnline = true });
                }
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User?.FindFirstValue("sub") ?? Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var friends = await _userGrpcService.GetListFriend(new GetListFriendRequest() { UserId = userId });
            if (!string.IsNullOrEmpty(userId))
            {
                Console.WriteLine($"User disconnected: {userId}, connection: {Context.ConnectionId}");
                await _connectionManager.SetUserOfflineAsync(userId, Context.ConnectionId);
            }

            foreach (var friend in friends.Friends)
            {
                var connections = await _connectionManager.GetConnectionIdsAsync(friend.UserName);
                foreach (var connectionId in connections)
                {
                    await Clients.Client(connectionId)
                        .SendAsync("friendStatusChanged", new { userName = userId, isOnline = false });
                }
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
                Console.WriteLine($"Received heartbeat from user: {userId}");
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