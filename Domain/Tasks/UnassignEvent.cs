using Domain.Repositories;
using Domain.Tasks.Enums;

namespace Domain.Tasks;

public class UnassignEvent: Event
{
    public Guid AssigneeId;
    public RepositoryMember? Assignee;

    public UnassignEvent(string title, Guid creatorId, Guid taskId, Guid assigneeId) 
        : base(title, EventType.ISSUE_UNASSIGNED, creatorId, taskId)
    {
        AssigneeId = assigneeId;
    }
}