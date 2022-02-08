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
        public bool Publish(PublishGeneric generic)
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
            channel.BasicPublish(generic.MessageName, generic.MessageName, props, body);
            return true;
        }

        public bool Publish<T>(PublishMessage<T> generic)
        {
            var factory = GetConnectionFactory();
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            var serialize = JsonSerializer.Serialize(generic.Message);

            var body = Encoding.UTF8.GetBytes(serialize);
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
            channel.BasicPublish(generic.MessageName, generic.MessageName, props, body);
            return true;
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
    }
}
