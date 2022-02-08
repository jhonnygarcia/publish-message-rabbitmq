namespace HttpIntegrationRabbitMq.Dto
{
    public class PublishMessage<T>
    {
        public string Application { get; set; }
        public T Message { get; set; }
        public string MessageName { get; set; }
        public string KeyMessage { get; set; }
        public string UserId { get; set; }
    }
}
