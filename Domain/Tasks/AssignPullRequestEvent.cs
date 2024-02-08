using Domain.Repositories;
using Domain.Tasks.Enums;

namespace Domain.Tasks;

public class AssignPullRequestEvent: Event
{
    public Guid AssigneeId;
    public RepositoryMember? Assignee;
    
    public AssignPullRequestEvent()
    {
    }
    
    public AssignPullRequestEvent(string title, Guid creatorId, Guid taskId, Guid assigneeId) 
        : base(title, EventType.PULL_REQUEST_ASSIGNED, creatorId, taskId)
    {
        AssigneeId = assigneeId;
    }
}