using Application.Shared;
using Domain.Notifications;

namespace Application.Notifications.Queries.FindUserNotifications;

public sealed record FindUserNotificationsQuery(Guid UserId) : IQuery<List<Notification>>;