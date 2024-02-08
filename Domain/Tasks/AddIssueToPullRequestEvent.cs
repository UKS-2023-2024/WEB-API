using Domain.Tasks.Enums;

namespace Domain.Tasks;

public class AddIssueToPullRequestEvent: Event
{
    public Guid IssueId;
    public Issue? Issue;

    public AddIssueToPullRequestEvent()
    {
    }

    public AddIssueToPullRequestEvent(string title, Guid creatorId, Guid taskId, Guid issueId) 
        : base(title, EventType.PULL_REQUEST_ISSUE_ADDED, creatorId, taskId)
    {
        IssueId = issueId;
    }
}