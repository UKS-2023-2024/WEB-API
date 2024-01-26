using Domain.Shared.Interfaces;

namespace Domain.Notifications.Interfaces;

public interface INotificationRepository: IBaseRepository<Notification>
{
    Task<List<Notification>> FindByUserId(Guid userId);
}