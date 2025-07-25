using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Presence.Hubs;
using StackExchange.Redis;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Presence.Services
{
    public class RedisPresenceSubscriber : BackgroundService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IHubContext<PresenceHub> _hubContext;
        private readonly ILogger<RedisPresenceSubscriber> _logger;

        public RedisPresenceSubscriber(
            IConnectionMultiplexer redis,
            IHubContext<PresenceHub> hubContext,
            ILogger<RedisPresenceSubscriber> logger)
        {
            _redis = redis;
            _hubContext = hubContext;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var subscriber = _redis.GetSubscriber();

            await subscriber.SubscribeAsync("presence:events", async (channel, message) =>
            {
                try
                {
                    _logger.LogInformation("📥 Received message from Redis: {Message}", message);

                    var payload = JsonConvert.DeserializeObject<PresenceMessage>(message!);

                    if (payload != null)
                    {
                        // Broadcast to SignalR clients
                        await _hubContext.Clients.All.SendAsync("PresenceUpdated", payload, cancellationToken: stoppingToken);
                        _logger.LogInformation("📡 Broadcasted to SignalR clients");
                    }
                    else
                    {
                        _logger.LogWarning("⚠️ Message parsing failed or null");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "❌ Error processing Redis message");
                }
            });

            _logger.LogInformation("✅ RedisPresenceSubscriber is listening on 'presence:events'");
        }

        private class PresenceMessage
        {
            public string UserId { get; set; } = "";
            public string Status { get; set; } = ""; 
            public string? ServerId { get; set; } 
        }
    }
}
