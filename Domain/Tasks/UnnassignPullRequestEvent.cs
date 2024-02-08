using Domain.Repositories;
using Domain.Tasks.Enums;

namespace Domain.Tasks;

public class UnnassignPullRequestEvent: Event
{
    public Guid AssigneeId;
    public RepositoryMember? Assignee;

    public UnnassignPullRequestEvent()
    {
    }
    public UnnassignPullRequestEvent(string title, Guid creatorId, Guid taskId, Guid assigneeId) 
        : base(title, EventType.PULL_REQUEST_UNASSIGNED, creatorId, taskId)
    {
        AssigneeId = assigneeId;
    }
}