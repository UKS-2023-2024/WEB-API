using Domain.Auth;
using Domain.Branches;
using Domain.Milestones;
using Domain.Repositories;
using Domain.Tasks.Enums;
using Domain.Tasks.Exceptions;
using Domain.Tasks.Interfaces;

namespace Domain.Tasks;

public class PullRequest : Task
{
    public Guid FromBranchId { get; private set; }
    public Branch? FromBranch { get; private set; }
    public Guid ToBranchId { get; private set; }
    public Branch? ToBranch { get; private set; }
    public int? GitPullRequestId { get; private set; }
    
    public List<Issue> Issues { get; protected set; } = new();

    private PullRequest(): base()
    {
    }

    public void SetGitPullRequestId(int gitPullRequestId)
    {
        GitPullRequestId = gitPullRequestId;
    }
    
    private PullRequest(string title, string description, int number, Guid userId, Guid repositoryId,
        List<RepositoryMember> assignees, List<Label> labels, Guid? milestoneId, Guid fromBranchId, Guid toBranchId, List<Issue> issues) : 
        base(title, description, TaskState.OPEN, number, TaskType.ISSUE, userId, repositoryId, assignees, labels, milestoneId)
    {
        FromBranchId = fromBranchId;
        ToBranchId = toBranchId;
        Events.Add(new Event("Opened pull request", EventType.OPENED, userId));
        UpdateAssignees(assignees, userId);
        UpdateIssues(issues, userId);
        UpdateLabels(labels, userId);
    }
    
    public static PullRequest Create(string title, string description, int number, Repository repository,
        Guid creatorId, List<RepositoryMember> assignees, List<Label> labels, Guid? milestoneId, Guid fromBranchId, Guid toBranchId,
        List<Issue> issues)
    {
        return new PullRequest(title, description, number, creatorId, repository.Id, assignees, labels,
            milestoneId, fromBranchId,  toBranchId, issues);
    }

    public static PullRequest Create(Guid id, string title, string description, int number, Repository repository,
       Guid creatorId, List<RepositoryMember> assignees, List<Label> labels, Guid? milestoneId, Guid fromBranchId, Guid toBranchId,
       List<Issue> issues)
    {
        PullRequest pr = new PullRequest(title, description, number, creatorId, repository.Id, assignees, labels,
            milestoneId, fromBranchId, toBranchId, issues);
        pr.Id = id;
        return pr;
    }

    public void UpdateAssignees(List<RepositoryMember> assignees, Guid creatorId)
    {
        CreateAddAssigneeEvents(assignees, creatorId);
        CreateRemoveAssigneeEvents(assignees, creatorId);
        Assignees = assignees;
    }
    
    public void UpdateIssues(List<Issue> issues, Guid creatorId)
    {
        CreateAddIssueEvents(issues, creatorId);
        CreateRemoveIssueEvents(issues, creatorId);
        Issues = issues;
    }
    
    public void UpdateMilestone(Milestone milestone, Guid creatorId)
    {
        if (Milestone is not null) UnassignMilestone(creatorId);
        MilestoneId = milestone.Id;
        Milestone = milestone;
        Events.Add(new AssignMilestoneEvent($"Added this pr to {milestone.Title}", creatorId, Id, milestone.Id));
    }
    
    public void UnassignMilestone(Guid creatorId)
    {
        if (Milestone is null) throw new PullRequestDoesNotHaveMilestoneException();
        Events.Add(new UnassignMilestoneEvent($"Removed this pr from {Milestone.Title}", creatorId, Id, Milestone.Id));
        MilestoneId = null;
        Milestone = null;
    }
    
    public void ClosePullRequest(Guid creatorId)
    {
        if (State == TaskState.CLOSED) throw new PullRequestClosedException("Pull request already closed!");
        if (State == TaskState.MERGED) throw new PullRequestMergedException("Pull request merged!");
        Events.Add(new CloseEvent("Closed pull request", creatorId, Id));
        State = TaskState.CLOSED;
    }
    
    public void ReopenPullRequest(Guid creatorId)
    {
        if (State == TaskState.OPEN) throw new PullRequestAlreadyOpenedException();
        if (State == TaskState.MERGED) throw new PullRequestMergedException("Pull request merged!");
        Events.Add(new Event("Reopened pull request", EventType.OPENED, creatorId));
        State = TaskState.OPEN;
    }
    
    public void MergePullRequest(Guid creatorId)
    {
        if (State == TaskState.CLOSED) throw new PullRequestClosedException("Pull request closed!");
        if (State == TaskState.MERGED) throw new PullRequestMergedException("Pull request already merged!");
        Events.Add(new PullRequestMergedEvent($"Pull request merged from branch {FromBranch.Name} to {ToBranch.Name}", creatorId, Id));
        State = TaskState.MERGED;
    }
    
    private void CreateAddAssigneeEvents(List<RepositoryMember> assignees, Guid creatorId)
    {
        if (assignees is null) return;
        foreach (var assigneeToAdd in assignees.Where(assignee => !Assignees.Contains(assignee)))
        {
            Events.Add(new AssignPullRequestEvent($"Assigned this pull request to {assigneeToAdd.Member.Username}", creatorId, Id, assigneeToAdd.Id));
        }
    }

    private void CreateRemoveAssigneeEvents(List<RepositoryMember> assignees, Guid creatorId)
    {
        if (assignees is null) return;
        foreach (var assigneeToRemove in Assignees.Where(assignee => !assignees.Contains(assignee)))
        {
            Events.Add(new UnnassignPullRequestEvent($"Removed {assigneeToRemove.Member.Username} from this pull request", creatorId, Id, assigneeToRemove.Id));
        }
    }
    
    private void CreateAddIssueEvents(List<Issue> issues, Guid creatorId)
    {
        if (issues is null) return;
        foreach (var issueToAdd in issues.Where(issue => !Issues.Contains(issue)))
        {
            Events.Add(new AddIssueToPullRequestEvent($"Added issue #{issueToAdd.Number} to pull request", creatorId, Id, issueToAdd.Id));
        }
    }

    private void CreateRemoveIssueEvents(List<Issue> issues, Guid creatorId)
    {
        if (issues is null) return;
        foreach (var issueToRemove in Issues.Where(issue => !issues.Contains(issue)))
        {
            Events.Add(new RemoveIssueFromPullRequestEvent($"Removed issue #{issueToRemove.Number} from pull request", creatorId, Id, issueToRemove.Id));
        }
    }

    public static void ThrowIfDoesntExist(PullRequest? pullRequest)
    {
        if (pullRequest is null) throw new PullRequestNotFoundException();
    }

    public void UpdateLabels(List<Label> labels, Guid creatorId)
    {
        CreateAddLabelEvents(labels, creatorId);
        CreateRemoveLabelEvents(labels, creatorId);
        Labels = labels;
    }

    private void CreateAddLabelEvents(List<Label> labels, Guid creatorId)
    {
        if (labels is null) return;
        foreach (var labelToAdd in labels.Where(label => !Labels.Contains(label)))
        {
            Events.Add(new AssignLabelEvent($"Added label {labelToAdd.Title} to pull request", creatorId, Id, labelToAdd.Id));
        }
    }

    private void CreateRemoveLabelEvents(List<Label> labels, Guid creatorId)
    {
        if (labels is null) return;
        foreach (var labelToRemove in Labels.Where(label => !labels.Contains(label)))
        {
            Events.Add(new UnassignLabelEvent($"Removed label {labelToRemove.Title} from pull request", creatorId, Id, labelToRemove.Id));
        }
    }
}