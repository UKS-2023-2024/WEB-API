using Domain.Repositories;

namespace Domain.Notifications.Interfaces;

public interface INotificationService
{
    Task SendNotification(Repository repository, string subject, string messaage);
   
}