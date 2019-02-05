using MassTransit;
using RabbitTest.Notifications;
using System;
using System.Diagnostics;

namespace RabbitTest
{
    public class Program
    {
        private static BusService _busService;

        public static void Main(string[] args)
        {
            try
            {
                Console.SetWindowSize(60, 10);

                BusConfig busConfig = new BusConfig("publisher", "1111", "rabbitmq://192.168.1.7:5672/");
                EndpointConvention.Map<StartNotification>(new Uri(busConfig.Host, "start"));
                EndpointConvention.Map<EndNotification>(new Uri(busConfig.Host, "end"));

                string consumer = args.Length > 0 ? args[0] : null;
                switch (consumer)
                {
                    case "start":
                        busConfig.ReceiveEndpoints.Add(new BusReceiveEndpoint("start", endpoint =>
                        {
                            endpoint.Handler<StartNotification>(async context =>
                            {
                                await Console.Out.WriteLineAsync($"Start message Received: {context.Message.ToString()}");
                                var endNotification = new EndNotification(context.Message);
                                context.Send(endNotification);
                            });
                        }));
                        break;
                    case "end":
                        busConfig.ReceiveEndpoints.Add(new BusReceiveEndpoint("end", endpoint =>
                        {
                            endpoint.Handler<EndNotification>(async context =>
                            {
                                await Console.Out.WriteLineAsync($"End message Received: {context.Message.ToString()}");
                            });
                        }));
                        break;
                    default:
                        break;
                }
                using (_busService = new BusService(busConfig))
                {
                    if (consumer != null)
                        Consumer(consumer);
                    else
                        Publisher();
                }
            }
            catch (Exception ex)
            {
                if (_busService != null)
                    _busService.Dispose();
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }

        private static void Consumer(string consumer)
        {
            Console.WriteLine("INIT CONSUMER - " + consumer);
            Console.WriteLine("\n");
            Console.WriteLine("KEYPRESS FOR QUIT.");
            Console.ReadKey();
        }

        private static void Publisher()
        {
            Console.WriteLine("INIT PUBLISHER");
            Console.WriteLine("\n");
            Console.WriteLine(" Q - Quit");
            Console.WriteLine(" M - NEW MESSAGE");
            Console.WriteLine(" 1 - NEW CONSUMER - start");
            Console.WriteLine(" 2 - NEW CONSUMER - end");

            while (true)
            {
                var key = Console.ReadKey();
                Console.WriteLine("\n");
                if (key.Key == ConsoleKey.M)
                {
                    var message = new StartNotification()
                    {
                        Id = Guid.NewGuid(),
                        DateTime = DateTime.Now,
                        Name = "Message"
                    };
                    _busService.BusControl.Send(message);
                    Console.WriteLine("NEW MESSAGE SENDED!");
                }

                if (key.Key == ConsoleKey.D1)
                    StartConsumer("start");

                if (key.Key == ConsoleKey.D2)
                    StartConsumer("end");

                if (key.Key == ConsoleKey.Q)
                    break;
            }
            Console.WriteLine("END...");
            Console.ReadKey();
        }

        private static void StartConsumer(string consumer)
        {
            ProcessStartInfo process = new ProcessStartInfo();
            process.UseShellExecute = true;
            process.CreateNoWindow = false;
            process.FileName = "dotnet";
            process.Arguments = "RabbitTest.dll " + consumer;
            Process.Start(process);

            Console.WriteLine("new consumer - " + consumer);
        }
    }
}
