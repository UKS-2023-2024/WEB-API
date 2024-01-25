using Domain.Auth;
using Domain.Notifications;
using Domain.Notifications.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Shared.Repositories;

namespace Infrastructure.Notifications.Repositories
{
    public class NotificationRepository : BaseRepository<Notification>, INotificationRepository
    {

        private readonly MainDbContext _context;
        public NotificationRepository(MainDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
