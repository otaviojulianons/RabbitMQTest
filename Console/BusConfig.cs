using System;
using System.Collections.Generic;

namespace RabbitTest
{
    public class BusConfig
    {
        public string Username { get; }
        public string Password { get; }
        public Uri Host { get; }
        public IList<BusReceiveEndpoint> ReceiveEndpoints { get; set; }

        public BusConfig(string username, string password, string host)
        {
            Username = username;
            Password = password;
            Host = new Uri(host);
            ReceiveEndpoints = new List<BusReceiveEndpoint>();
        }

    }
}
