using Domain.Repositories;
using Domain.Tasks.Enums;

namespace Domain.Tasks;

public class CloseEvent: Event
{
   
    public CloseEvent()
    {
    }
    public CloseEvent(string title, Guid creatorId, Guid taskId) 
        : base(title, EventType.CLOSED, creatorId, taskId)
    {
    }
}