using Domain.Repositories;
using Domain.Tasks.Enums;

namespace Domain.Tasks;

public class AssignEvent: Event
{
    public Guid AssigneeId { get; set; }
    public RepositoryMember? Assignee { get; set; }

    public AssignEvent(string title, Guid creatorId, Guid taskId, Guid assigneeId) 
        : base(title, EventType.ISSUE_ASSIGNED, creatorId, taskId)
    {
        AssigneeId = assigneeId;
    }
}