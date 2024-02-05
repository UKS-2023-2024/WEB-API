using Application.Shared;
using Domain.Auth;
using Domain.Auth.Enums;

namespace Application.Notifications.Commands.UpdateNotificationPreferences;
public sealed record UpdateNotificationPreferencesCommand(Guid userId, NotificationPreferences preferences) : ICommand<User>;
