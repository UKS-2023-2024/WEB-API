using Domain.Shared;
using Domain.Shared.Interfaces;

namespace Domain.Notifications.Interfaces;

public interface INotificationRepository: IBaseRepository<Notification>
{
    Task<PagedResult<Notification>> FindByUserId(Guid userId, int pageSize, int pageNumber);
}