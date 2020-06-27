using System;

namespace RabbitTest.Notifications
{
    public class EndNotification2
    {
        public EndNotification2()
        {

        }

        public EndNotification2(StartNotification notification)
        {
            this.StartNotification = notification;
            this.DateTimeProcess = DateTime.Now;
        }

        public StartNotification StartNotification { get; set; }
        public DateTime DateTimeProcess { get; set; }

        public override string ToString()
        {
            return "### End - " + StartNotification?.ToString();
        }
    }
}
