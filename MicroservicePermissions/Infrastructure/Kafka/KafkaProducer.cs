using Confluent.Kafka;
using MicroservicePermissions.Application.DTOs;
using MicroservicePermissions.Application.Interfaces;
using System.Text.Json;


namespace MicroservicePermissions.Infrastructure.Kafka
{
    public class KafkaProducer : IKafkaProducer
    {
        private readonly string _bootstrapServers;
        private readonly string _topic;

        public KafkaProducer(IConfiguration configuration)
        {
            _bootstrapServers = configuration["Kafka:BootstrapServers"] ?? "localhost:9092";
            _topic = configuration["Kafka:Topic"] ?? "operations-log";
        }

        public async Task SendMessageAsync<T>(KafkaMessageDto<T> message)
        {
            var config = new ProducerConfig { BootstrapServers = _bootstrapServers };

            using var producer = new ProducerBuilder<Null, string>(config).Build();

            var value = JsonSerializer.Serialize(message);

            await producer.ProduceAsync(_topic, new Message<Null, string> { Value = value });

            producer.Flush(TimeSpan.FromSeconds(2));
        }
    }
}
