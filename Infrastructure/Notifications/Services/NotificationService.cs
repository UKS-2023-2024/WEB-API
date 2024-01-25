using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Notifications;
using Domain.Notifications.Interfaces;
using Domain.Repositories;
using Domain.Shared.Interfaces;


namespace Infrastructure.Notifications.Services
{
    public class NotificationService : INotificationService
    {
        private IEmailService _emailService;
        private INotificationRepository _notificationRepository;
        public NotificationService(IEmailService emailService, INotificationRepository notificationRepository)
        {
            _emailService = emailService;
            _notificationRepository = notificationRepository;
        }
        public Task SendIssueHasBeenOpenedNotification(Repository repository, Domain.Tasks.Issue issue)
        {
            foreach (User watcher in repository.WatchedBy)
            {
                if (watcher.NotificationPreferences == NotificationPreferences.EMAIL || watcher.NotificationPreferences == NotificationPreferences.BOTH)
                {
                    _emailService.SendNotificationIssueIsOpen(watcher, issue, repository.Name);
                }
                if (watcher.NotificationPreferences == NotificationPreferences.GITHUB || watcher.NotificationPreferences == NotificationPreferences.BOTH)
                {
                    string message = $"A new issue has been opened in the repository {repository.Name}<br><br>" +
                                     $"Title: {issue.Title} <br>" +
                                     $"Description: {issue.Description}<br>" +
                                     $"Opened by: {issue.Creator?.Username}";
                    Notification notification = Notification.Create(message, watcher, DateTime.UtcNow);
                    Notification n = _notificationRepository.Create(notification).Result;
                    Console.WriteLine(n);
                }
            }
            return Task.CompletedTask;
        }
    }
}
