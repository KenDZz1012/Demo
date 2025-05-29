using Channel.Application.Contracts.Persistence;
using Channel.Shared.Contracts;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Channel.Infrastructure.Kafka
{
    public class UserValidationProducer : IUserValidationService
    {
        private readonly IProducer<string, string> _producer;

        public UserValidationProducer(IConfiguration config)
        {
            var kafkaConfig = new ProducerConfig
            {
                BootstrapServers = config["Kafka:BootstrapServers"]
            };
            _producer = new ProducerBuilder<string, string>(kafkaConfig).Build();
        }

        public async Task RequestUserValidationAsync(Guid userId, string correlationId)
        {
            var message = new UserValidationRequest
            {
                CorrelationId = correlationId,
                UserId = userId,
                ReplyTopic = "user.validation.response"
            };

            var json = JsonSerializer.Serialize(message);
            await _producer.ProduceAsync("user.validation.request", new Message<string, string>
            {
                Key = correlationId,
                Value = json
            });
        }
    }

}
