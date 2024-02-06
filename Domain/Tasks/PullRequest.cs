using Domain.Auth;
using Domain.Branches;
using Domain.Repositories;
using Domain.Tasks.Enums;
using Domain.Tasks.Interfaces;

namespace Domain.Tasks;

public class PullRequest : Task
{
    public Guid FromBranchId { get; private set; }
    public Branch? FromBranch { get; private set; }
    public Guid ToBranchId { get; private set; }
    public Branch? ToBranch { get; private set; }
    public int? GitPullRequestId { get; private set; }

    public void SetGitPullRequestId(int gitPullRequestId)
    {
        GitPullRequestId = gitPullRequestId;
    }
    
    private PullRequest(string title, string description, TaskState state, int number, Guid userId, Guid repositoryId,
        List<RepositoryMember> assignees, List<Label> labels, Guid? milestoneId, Guid fromBranchId, Guid toBranchId) : 
        base(title, description, state, number, TaskType.ISSUE, userId, repositoryId, assignees, labels, milestoneId)
    {
        FromBranchId = fromBranchId;
        ToBranchId = toBranchId;
        Events.Add(new Event("Opened pull request", EventType.OPENED, userId));
        UpdateAssignees(assignees, userId);
    }
    
    public static PullRequest Create(string title, string description, TaskState state, int number, Repository repository,
        Guid creatorId, List<RepositoryMember> assignees, List<Label> labels, Guid? milestoneId, Guid fromBranchId, Guid toBranchId)
    {
        return new PullRequest(title, description, state, number, creatorId, repository.Id, assignees, labels,
            milestoneId, fromBranchId,  toBranchId);
    }
    
    public void UpdateAssignees(List<RepositoryMember> assignees, Guid creatorId)
    {
        CreateAddAssigneeEvents(assignees, creatorId);
        CreateRemoveAssigneeEvents(assignees, creatorId);
        Assignees = assignees;
    }
    
    public void UpdateMilestone(Guid milestoneId, Guid creatorId)
    {
        MilestoneId = milestoneId;
        Events.Add(new AssignMilestoneEvent("Milestone Assigned", creatorId, Id, milestoneId));
    }
    
    public void ClosePullRequest(Guid creatorId)
    {
        Events.Add(new Event("Closed pull request", EventType.OPENED, creatorId));
    }
    
    public void ReopenPullRequest(Guid creatorId)
    {
        Events.Add(new Event("Reopened pull request", EventType.OPENED, creatorId));
    }
    
    public void MergePullRequest(Guid creatorId)
    {
        Events.Add(new Event($"Pull request merged from branch {FromBranch.Name} to {ToBranch.Name}",
            EventType.PULL_REQUEST_MERGED, creatorId));
    }
    
    private void CreateAddAssigneeEvents(List<RepositoryMember> assignees, Guid creatorId)
    {
        if (assignees is null) return;
        foreach (var assignee in assignees.Where(assignee => !Assignees.Contains(assignee)))
        {
            Events.Add(new AssignPullRequestEvent("Assigned", creatorId, Id, assignee.Id));
        }
    }

    private void CreateRemoveAssigneeEvents(List<RepositoryMember> assignees, Guid creatorId)
    {
        if (assignees is null) return;
        foreach (var assignee in Assignees.Where(assignee => !assignees.Contains(assignee)))
        {
            Events.Add(new UnnassignPullRequestEvent("Unassigned", creatorId, Id, assignee.Id));
        }
    }
}