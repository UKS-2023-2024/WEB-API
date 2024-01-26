using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Notifications;
using Domain.Notifications.Interfaces;
using Domain.Repositories;
using Domain.Repositories.Enums;
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
        public Task SendNotification(Repository repository, string subject, string message, NotificationType notificationType)
        {
            foreach (RepositoryWatcher watcher in repository.WatchedBy)
            {
                if (watcher.WatchingPreferences == WatchingPreferences.Ignore) return Task.CompletedTask;
                if (ShouldUserBeNotified(watcher, notificationType)) 
                { 
                    if (watcher.User.NotificationPreferences == NotificationPreferences.EMAIL || watcher.User.NotificationPreferences == NotificationPreferences.BOTH)
                    {
                        _emailService.SendNotificationToRepositoryWatcher(watcher.User, subject, message);
                    }
                    if (watcher.User.NotificationPreferences == NotificationPreferences.GITHUB || watcher.User.NotificationPreferences == NotificationPreferences.BOTH)
                    {
                        Notification notification = Notification.Create(message, watcher.User, DateTime.UtcNow);
                        _notificationRepository.Create(notification);
                    }
                }
            }
            return Task.CompletedTask;
        }

        private bool ShouldUserBeNotified(RepositoryWatcher watcher, NotificationType notificationType)
        {
            WatchingPreferences pref = watcher.WatchingPreferences;
            if (pref == WatchingPreferences.AllActivity || pref == WatchingPreferences.IssuesAndPullRequests) return true;
            return pref.ToString() == notificationType.ToString();
        }
    }
}
