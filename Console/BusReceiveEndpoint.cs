using MassTransit.RabbitMqTransport;
using System;

namespace RabbitTest
{
    public class BusReceiveEndpoint
    {
        public string QueueName { get; private set; }

        public Action<IRabbitMqReceiveEndpointConfigurator> Action { get; private set; }

        public BusReceiveEndpoint(string queueName, Action<IRabbitMqReceiveEndpointConfigurator> action)
        {
            QueueName = queueName;
            Action = action;
        }
    }
}
