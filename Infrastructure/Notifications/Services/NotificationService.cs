using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Notifications;
using Domain.Notifications.Interfaces;
using Domain.Repositories;
using Domain.Repositories.Enums;
using Domain.Shared.Interfaces;
using MediatR;

namespace Infrastructure.Notifications.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IMediator _mediator;
        public NotificationService(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task SendNotification(Repository repository, string subject, string message, NotificationType notificationType)
        {
            foreach (RepositoryWatcher watcher in repository.WatchedBy)
            {
                if (ShouldUserBeNotified(watcher, notificationType)) 
                {
                    await _mediator.Publish(Notification.Create(message, subject, watcher.User, DateTime.UtcNow));
                }
            }
        }

        private bool ShouldUserBeNotified(RepositoryWatcher watcher, NotificationType notificationType)
        {
            WatchingPreferences pref = watcher.WatchingPreferences;
            if (pref == WatchingPreferences.Ignore) return false;
            if (pref == WatchingPreferences.AllActivity || pref == WatchingPreferences.IssuesAndPullRequests) return true;
            return pref.ToString() == notificationType.ToString();
        }
    }
}
