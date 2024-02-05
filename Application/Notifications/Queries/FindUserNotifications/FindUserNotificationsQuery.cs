using Application.Shared;
using Domain.Notifications;
using Domain.Shared;

namespace Application.Notifications.Queries.FindUserNotifications;

public sealed record FindUserNotificationsQuery(Guid UserId, int pageSize, int pageNumber) : IQuery<PagedResult<Notification>>;