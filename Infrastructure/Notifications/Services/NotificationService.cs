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
        public Task SendNotification(Repository repository, string subject, string message)
        {
            foreach (User watcher in repository.WatchedBy)
            {
                if (watcher.NotificationPreferences == NotificationPreferences.EMAIL || watcher.NotificationPreferences == NotificationPreferences.BOTH)
                {
                    _emailService.SendNotificationToRepositoryWatcher(watcher, subject, message);
                }
                if (watcher.NotificationPreferences == NotificationPreferences.GITHUB || watcher.NotificationPreferences == NotificationPreferences.BOTH)
                {
                    Notification notification = Notification.Create(message, watcher, DateTime.UtcNow);
                   _notificationRepository.Create(notification);
                }
            }
            return Task.CompletedTask;
        }
    }
}
