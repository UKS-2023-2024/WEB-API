using Domain.Notifications;
using Domain.Notifications.Interfaces;
using Domain.Organizations.Interfaces;
using Domain.Shared;
using MediatR;

namespace Application.Notifications.Queries.FindUserNotifications;

public class FindUserNotificationsQueryHandler: IRequestHandler<FindUserNotificationsQuery, PagedResult<Notification>>
{
    private readonly INotificationRepository _notificationRepository;

    public FindUserNotificationsQueryHandler(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task<PagedResult<Notification>> Handle(FindUserNotificationsQuery request, CancellationToken cancellationToken)
    {
        return await _notificationRepository.FindByUserId(request.UserId, request.pageSize, request.pageNumber);
    }
}