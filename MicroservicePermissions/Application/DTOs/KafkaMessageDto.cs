namespace MicroservicePermissions.Application.DTOs
{
    public class KafkaMessageDto<T>
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Operation { get; set; } = string.Empty;
        public T Data { get; set; } = default!;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;


    }
}
