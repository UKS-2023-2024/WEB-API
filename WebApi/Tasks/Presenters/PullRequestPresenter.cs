using Domain.Milestones;
using Domain.Repositories;
using Domain.Tasks;
using Domain.Tasks.Enums;
using WEB_API.Milestones.Presenters;
using WEB_API.Repositories.Presenters;

namespace WEB_API.Tasks.Presenters;

public class PullRequestPresenter
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string RepositoryId { get; set; }
    public int Number { get; set; }
    public TaskState State { get; set; }
    public string fromBranch { get; set; }
    public string toBranch { get; set; }
    public List<EventPresenter> Events { get; set; }
    public IEnumerable<RepositoryMemberPresenter> Assignees { get; set; }
    public List<IssuePullRequestPresenter> issues { get; set; }
    public MilestonePresenter Milestone { get; set; }

    public PullRequestPresenter(PullRequest pullRequest)
    {
        Id = pullRequest.Id.ToString();
        Title = pullRequest.Title;
        Description = pullRequest.Description;
        RepositoryId = pullRequest.RepositoryId.ToString();
        Number = pullRequest.Number;
        Events = EventPresenter.MapEventToEventPresenter(pullRequest.Events);
        Assignees = RepositoryMemberPresenter.MapRepositoryMembersToPresenters(pullRequest.Assignees);
        State = pullRequest.State;
        fromBranch = pullRequest.FromBranch.Name;
        toBranch = pullRequest.ToBranch.Name;
        issues = IssuePullRequestPresenter.MapEventToEventPresenter(pullRequest.Issues);
        if (pullRequest.Milestone is not null)
            Milestone = new MilestonePresenter(pullRequest.Milestone);
    }

    public static List<PullRequestPresenter> MapPullRequestToPullRequestPresenter(List<PullRequest> pullRequests)
    {
        return pullRequests.Select(pullRequest => new PullRequestPresenter(pullRequest)).ToList();
    }
}