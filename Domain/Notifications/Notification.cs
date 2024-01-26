using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Organizations;
using Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Domain.Notifications
{
    public class Notification : INotification
    {
        public Guid Id { get; private set; }
        public string Subject { get; private set; }
        public string Message { get; private set; }
        public Guid UserId { get; private set; }
        public User User { get; private set; }
        public DateTime DateTime { get; private set; }

        private Notification() { }

        private Notification(string message, string subject, User user, DateTime dateTime)
        {
            Subject = subject;
            Message = message;
            User = user;
            DateTime = dateTime;
        }

        public static Notification Create(string message, string subject, User user, DateTime dateTime)
        {
            return new Notification(message, subject, user, dateTime);
        }
    }
}
