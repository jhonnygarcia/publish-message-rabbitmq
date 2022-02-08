namespace HttpIntegrationRabbitMq.Dto
{
    public class PublishToQueue : PublishGeneric
    {
        public string QueueName { get; set; }
    }
}
