using System;

namespace RabbitTest.Notifications
{
    public class StartNotification
    {
        public Guid Id { get; set; }

        public DateTime DateTime { get; set; }

        public string Name { get; set; }

        public override string ToString()
        {
            return Name + " - " + DateTime.ToFileTimeUtc();
        }

    }
}
