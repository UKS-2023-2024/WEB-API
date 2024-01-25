using Domain.Repositories;

namespace Domain.Notifications.Interfaces;

public interface INotificationService
{
    Task SendIssueHasBeenOpenedNotification(Repository repository, Tasks.Issue issue);
   
}