using System;

namespace RabbitTest.Notifications
{
    public class EndNotification : StartNotification
    {
        public EndNotification()
        {

        }

        public EndNotification(StartNotification notification)
        {
            this.DateTime = notification.DateTime;
            this.Name = notification.Name;
            this.Id = notification.Id;
            this.DateTimeProcess = DateTime.Now;
        }

        public DateTime DateTimeProcess { get; set; }
    }
}
