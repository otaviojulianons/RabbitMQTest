using MassTransit;
using MassTransit.RabbitMqTransport;
using RabbitTest.Notifications;
using System;

namespace RabbitTest
{
    public class BusService : IDisposable
    {
        public IBusControl BusControl { get; set; }

        private readonly BusConfig _busConfig;


        public BusService(BusConfig busConfig)
        {
            _busConfig = busConfig;

            BusControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(_busConfig.Host, h =>
                {
                    h.Username(_busConfig.Username);
                    h.Password(_busConfig.Password);
                });

                foreach(var endpoint in _busConfig.ReceiveEndpoints)
                    cfg.ReceiveEndpoint(host, endpoint.QueueName, endpoint.Action);

            });

            BusControl.Start();
        }


        public void Dispose()
        {
            BusControl?.Stop();
        }
    }
}
