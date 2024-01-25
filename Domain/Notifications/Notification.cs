using Domain.Auth;
using Domain.Organizations;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Domain.Notifications
{
    public class Notification
    {
        public Guid Id { get; private set; }
        public string Message { get; private set; }
        public Guid UserId { get; private set; }
        public User User { get; private set; }
        public DateTime DateTime { get; private set; }

        private Notification() { }

        private Notification(string message, Guid userId, DateTime dateTime)
        {
            Message = message;
            UserId = userId;
            DateTime = dateTime;
        }

        public static Notification Create(string message, Guid userId, DateTime dateTime)
        {
            return new Notification(message, userId, dateTime);
        }
    }
}
