using Domain.Tasks;
using Domain.Tasks.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Shared.Repositories;

namespace Infrastructure.Events;

public class EventRepository: BaseRepository<Event>, IEventRepository
{
    public EventRepository(MainDbContext context) : base(context)
    {
    }
}