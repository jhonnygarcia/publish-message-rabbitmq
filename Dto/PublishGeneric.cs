namespace HttpIntegrationRabbitMq.Dto
{
    public class PublishGeneric
    {
        public string Application { get; set; }
        public string ContentMessage { get; set; }
        public string MessageName { get; set; }
        public string KeyMessage { get; set; }
        public string UserId { get; set; }
    }
}
