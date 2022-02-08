using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using HttpIntegrationRabbitMq.Dto;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace HttpIntegrationRabbitMq
{
    public class BusClientService: IBusClient
    {
        private BusConfigurationModel BusConfig { get; }
        public BusClientService(IOptions<BusConfigurationModel> busConfig)
        {
            BusConfig = busConfig.Value;
        }
        public bool Publish(PublishGeneric generic, string queueName)
        {
            return PublishMessage(generic, queueName);
        }
        public bool Publish(PublishGeneric generic)
        {
            return PublishMessage(generic);
        }
        public bool Publish<T>(PublishMessage<T> generic)
        {
            return Publish(new PublishGeneric
            {
                MessageName = generic.MessageName,
                Application = generic.Application,
                ContentMessage = JsonSerializer.Serialize(generic.Message),
                KeyMessage = generic.KeyMessage,
                UserId = generic.UserId
            });
        }
        public bool Publish<T>(PublishMessage<T> generic, string queueName)
        {
            return Publish(new PublishGeneric
            {
                MessageName = generic.MessageName,
                Application = generic.Application,
                ContentMessage = JsonSerializer.Serialize(generic.Message),
                KeyMessage = generic.KeyMessage,
                UserId = generic.UserId
            }, queueName);
        }

        private ConnectionFactory GetConnectionFactory()
        {
            var factory = new ConnectionFactory
            {
                HostName = BusConfig.Host,
                UserName = BusConfig.Username,
                Password = BusConfig.Password,
                VirtualHost = BusConfig.VirtualHost
            };
            if (!string.IsNullOrWhiteSpace(BusConfig.Port))
            {
                factory.Port = int.Parse(BusConfig.Port);
            }

            return factory;
        }
        private bool PublishMessage(PublishGeneric generic, string queueName = null)
        {
            var factory = GetConnectionFactory();
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            var body = Encoding.UTF8.GetBytes(generic.ContentMessage);
            var props = channel.CreateBasicProperties();
            props.Headers = new Dictionary<string, object>
            {
                { "application", generic.Application },
                {
                    "key", string.IsNullOrWhiteSpace(generic.KeyMessage) ?
                        Guid.NewGuid().ToString() : generic.KeyMessage
                },
                { "accountId", generic.UserId },
                { "name", generic.MessageName }
            };
            var queue = string.IsNullOrWhiteSpace(queueName) ? queueName : generic.MessageName;
            channel.BasicPublish(generic.MessageName, queue, props, body);
            return true;
        }
    }
}
