using Domain.Notifications;
using Domain.Notifications.Interfaces;
using Domain.Organizations.Interfaces;
using MediatR;

namespace Application.Notifications.Queries.FindUserNotifications;

public class FindUserNotificationsQueryHandler: IRequestHandler<FindUserNotificationsQuery, List<Notification>>
{
    private readonly INotificationRepository _notificationRepository;

    public FindUserNotificationsQueryHandler(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task<List<Notification>> Handle(FindUserNotificationsQuery request, CancellationToken cancellationToken)
    {
        return await _notificationRepository.FindByUserId(request.UserId);
    }
}