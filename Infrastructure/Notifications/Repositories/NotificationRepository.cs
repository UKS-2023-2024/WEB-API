using Domain.Auth;
using Domain.Notifications;
using Domain.Notifications.Interfaces;
using Domain.Repositories;
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

        public async Task<List<Notification>> FindByUserId(Guid userId)
        {
            return _context.Notifications
            .Where(n => n.UserId.Equals(userId))
            .ToList();
        }
    }
}
