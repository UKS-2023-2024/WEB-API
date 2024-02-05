using Domain.Auth;
using Domain.Branches;
using Domain.Notifications;
using Domain.Notifications.Interfaces;
using Domain.Repositories;
using Domain.Shared;
using Infrastructure.Persistence;
using Infrastructure.Shared.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Notifications.Repositories
{
    public class NotificationRepository : BaseRepository<Notification>, INotificationRepository
    {

        private readonly MainDbContext _context;
        public NotificationRepository(MainDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PagedResult<Notification>> FindByUserId(Guid userId, int pageSize, int pageNumber)
        {
            var query = _context.Notifications
            .Where(n => n.UserId.Equals(userId));

            var totalItems = await query.CountAsync();

            var data = await query
                  .OrderByDescending(n => n.DateTime)
                  .Skip((pageNumber - 1) * pageSize)
                  .Take(pageSize)
                  .ToListAsync();

            return new PagedResult<Notification>(data, totalItems);
        }
    }
}
