using Domain.Auth;
using Domain.Tasks;
using Domain.Tasks.Enums;

namespace WEB_API.Tasks.Presenters;

public class EventPresenter
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public string Creator { get; private set; }
    public EventType EventType { get; private set; }
    public Guid TaskId { get; private set; }

    public EventPresenter(Event @event)
    {
        Id = @event.Id;
        Title = @event.Title;
        CreatedAt = @event.CreatedAt;
        Creator = @event.Creator.Username;
        EventType = @event.EventType;
        TaskId = @event.TaskId;
    }
    
    public static List<EventPresenter> MapEventToEventPresenter(List<Event> events)
    {
        return events.Select(@event => new EventPresenter(@event)).ToList();
    }
}