using MicroservicePermissions.Application.DTOs;

namespace MicroservicePermissions.Application.Interfaces
{
    public interface IKafkaProducer
    {
        Task SendMessageAsync<T>(KafkaMessageDto<T> message);
    }
}
