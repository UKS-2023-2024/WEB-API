using Domain.Notifications;

namespace WEB_API.Notifications.Presenters;

public class NotificationPresenter
{
    public Guid Id { get; set; }
    public string Message { get; set; }
    public DateTime DateTime { get; set; }


    public NotificationPresenter(Notification notification)
    {
        Id = notification.Id;
        Message = notification.Message;
        DateTime = notification.DateTime;
    }

    public static List<NotificationPresenter> MapNotificationsToNotificationPresenters(
        List<Notification> notifications)
    {
        return notifications.Select(n => new NotificationPresenter(n)).ToList();
    }
}