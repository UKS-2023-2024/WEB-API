using Domain.Milestones;
using Domain.Repositories;
using Domain.Tasks;
using Domain.Tasks.Enums;

namespace WEB_API.Tasks.Presenters;

public class PullRequestPresenter
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string RepositoryId { get; set; }
    public List<RepositoryMember> Assignees { get; set; }
    public List<Label> Labels { get; set; }
    public Milestone? Milestone { get; set; }
    public int Number { get; set; }
    public TaskState State { get; set; }
    public string fromBranch { get; set; }
    public string toBranch { get; set; }
    public List<Event> Events { get; set; }
    
    public List<IssuePresenter> issues { get; set; }

    public PullRequestPresenter(PullRequest pullRequest)
    {
        Id = pullRequest.Id.ToString();
        Title = pullRequest.Title;
        Description = pullRequest.Description;
        RepositoryId = pullRequest.RepositoryId.ToString();
        Assignees = pullRequest.Assignees;
        Labels = pullRequest.Labels;
        Milestone = pullRequest.Milestone;
        Number = pullRequest.Number;
        Events = pullRequest.Events;
        State = pullRequest.State;
        fromBranch = pullRequest.FromBranch.Name;
        toBranch = pullRequest.ToBranch.Name;
        issues = IssuePresenter.MapIssueToIssuePresenter(pullRequest.Issues);
    }

    public static List<PullRequestPresenter> MapPullRequestToPullRequestPresenter(List<PullRequest> pullRequests)
    {
        return pullRequests.Select(pullRequest => new PullRequestPresenter(pullRequest)).ToList();
    }
}