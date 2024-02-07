using Domain.Tasks.Enums;

namespace Domain.Tasks;

public class RemoveIssueFromPullRequestEvent: Event
{
    public Guid IssueId;
    public Issue? Issue;
    
    public RemoveIssueFromPullRequestEvent()
    {
    }
    public RemoveIssueFromPullRequestEvent(string title, Guid creatorId, Guid taskId, Guid issueId) 
        : base(title, EventType.PULL_REQUEST_ISSUE_REMOVED, creatorId, taskId)
    {
        IssueId = issueId;
    }
}