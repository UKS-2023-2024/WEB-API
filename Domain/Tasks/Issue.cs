using Domain.Auth;
using Domain.Repositories;
using Domain.Tasks.Enums;

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

    public static Issue Update(Issue issue, string title, string description, TaskState state,
        List<RepositoryMember> assignees, Guid milestoneId, List<Label> labels)
    {
        
        issue.Title = title;
        issue.Description = description;
        issue.State = state;
        issue.Assignees = assignees;
        issue.MilestoneId = milestoneId;
        issue.Labels = labels;
        return issue;
    }

    public void UpdateAssignees(List<RepositoryMember> assignees, Guid creatorId)
    {
        CreateAddAssigneeEvents(assignees, creatorId);
        CreateRemoveAssigneeEvents(assignees, creatorId);
        Assignees = assignees;
    }
    private void CreateAddAssigneeEvents(List<RepositoryMember> assignees, Guid creatorId)
    {
        foreach (RepositoryMember assignee in assignees)
        {
            if(!Assignees.Contains(assignee))
                Events.Add(new AssignEvent("Assigned", creatorId, Id, assignee.Id));
        }
    }

    private void CreateRemoveAssigneeEvents(List<RepositoryMember> assignees, Guid creatorId)
    {
        foreach (RepositoryMember assignee in Assignees)
        {
            if(!assignees.Contains(assignee))
                Events.Add(new UnassignEvent("Unassigned", creatorId, Id, assignee.Id));
        }
    
    }
    

}