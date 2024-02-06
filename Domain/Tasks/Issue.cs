using Domain.Auth;
using Domain.Repositories;
using Domain.Tasks.Enums;
using Domain.Tasks.Interfaces;

namespace Domain.Tasks;

public class Issue: Task
{
    private Issue(): base()
    {
    }

    public Issue(string title, string description, TaskState state, int number, Guid userId, Guid repositoryId,
        List<RepositoryMember> assignees, List<Label> labels, Guid? milestoneId) : 
        base(title, description, state, number, TaskType.ISSUE, userId, repositoryId, assignees, labels, milestoneId)
    {
        Events.Add(new Event("Opened issue", EventType.OPENED, userId));
        UpdateAssignees(assignees, userId);
    }
    public Issue(string title, string description, TaskState state, int number, Guid userId, Guid repositoryId) : 
        base(title, description, state, number, TaskType.ISSUE, userId, repositoryId)
    {
        Events.Add(new Event("Opened issue", EventType.OPENED, userId));
    }
    
    public static Issue Create(string title, string description, TaskState state, int number, Repository repository,
        User creator, List<RepositoryMember> assignees, List<Label> labels, Guid? milestoneId)
    {
        return new Issue(title, description, state, number, creator.Id, repository.Id, assignees, labels, milestoneId);
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

    public void UnassignMilestone(Guid milestoneId, Guid creatorId, Guid currentMilestoneId)
    {
        Events.Add(new UnassignMilestoneEvent("Milestone Unassigned", creatorId, Id, currentMilestoneId));
        if (milestoneId == Guid.Parse("00000000-0000-0000-0000-000000000000")) return;
        UpdateMilestone(milestoneId, creatorId);
    }

    private void CreateAddAssigneeEvents(List<RepositoryMember> assignees, Guid creatorId)
    {
        if (assignees is null) return;
        foreach (RepositoryMember assignee in assignees)
        {
            if(!Assignees.Contains(assignee))
                Events.Add(new AssignEvent("Assigned", creatorId, Id, assignee.Id));
        }
    }

    private void CreateRemoveAssigneeEvents(List<RepositoryMember> assignees, Guid creatorId)
    {
        if (assignees is null) return;
        foreach (RepositoryMember assignee in Assignees)
        {
            if(!assignees.Contains(assignee))
                Events.Add(new UnassignEvent("Unassigned", creatorId, Id, assignee.Id));
        }
    
    }
}