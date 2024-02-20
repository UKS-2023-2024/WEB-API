using Domain.Repositories;
using Domain.Tasks.Enums;

namespace Domain.Tasks;

public class PullRequestMergedEvent: Event
{
   
    public PullRequestMergedEvent()
    {
    }
    public PullRequestMergedEvent(string title, Guid creatorId, Guid taskId) 
        : base(title, EventType.PULL_REQUEST_MERGED, creatorId, taskId)
    {
    }
}