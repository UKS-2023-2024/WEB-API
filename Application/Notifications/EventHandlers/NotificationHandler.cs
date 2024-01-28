using Domain.Auth.Enums;
using Domain.Notifications;
using Domain.Notifications.Interfaces;
using Domain.Repositories.Interfaces;
using Domain.Shared.Interfaces;
using MediatR;

namespace Application.Notifications.EventHandlers;

public class NotificationHandler : INotificationHandler<Notification>
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IEmailService _emailService;
    public NotificationHandler(INotificationRepository notificationRepository, IEmailService emailService)
    {
        _notificationRepository = notificationRepository;
        _emailService = emailService;
    }
    public async Task Handle(Notification notification, CancellationToken cancellationToken)
    {

        if (notification.User.NotificationPreferences == NotificationPreferences.EMAIL || notification.User.NotificationPreferences == NotificationPreferences.BOTH)
        {
            await _emailService.SendNotificationToRepositoryWatcher(notification.User, notification.Subject, notification.Message);
        }
        if (notification.User.NotificationPreferences == NotificationPreferences.GITHUB || notification.User.NotificationPreferences == NotificationPreferences.BOTH)
        {
            await _notificationRepository.Create(notification);
        }
    }
}