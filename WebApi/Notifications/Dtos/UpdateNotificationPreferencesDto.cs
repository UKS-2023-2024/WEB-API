using Domain.Tasks.Enums;

namespace WEB_API.Notifications.Dtos;

public class UpdateNotificationPreferencesDto
{
    public bool Email { get; set; }

    public bool Github { get;  set; }

}