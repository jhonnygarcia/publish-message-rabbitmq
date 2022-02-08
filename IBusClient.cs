using HttpIntegrationRabbitMq.BusMessages;
using HttpIntegrationRabbitMq.Dto;

namespace HttpIntegrationRabbitMq
{
    public interface IBusClient
    {
        bool Publish(PublishGeneric generic);
        bool Publish<T>(PublishMessage<T> generic);
    }
}
