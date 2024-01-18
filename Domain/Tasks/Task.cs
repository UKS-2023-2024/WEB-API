using Domain.Auth;
using Domain.Milestones;
using Domain.Repositories;
using Domain.Tasks.Enums;

namespace Domain.Tasks;

public abstract class Task
{
    public Guid Id { get; protected set; }
    public string Title { get; protected set; }
    public string Description { get; protected set; }
    public TaskState State { get; protected set; }
    public int Number { get; protected set; }
    public Guid RepositoryId { get; protected set; }
    public Repository? Repository { get; protected set; }
    public List<RepositoryMember> Assignees { get; protected set; } = new();
    public List<Label> Labels { get; protected set; } = new();
    public Guid? MilestoneId { get; protected set; }
    public Milestone? Milestone { get; protected set; }
    public TaskType Type { get; protected set; }
    public List<Event> Events { get; set; } = new();
    public Guid UserId { get; protected set; }
    public User? Creator { get; protected set; }

    protected Task()
    {
    }

    protected Task(string title, string description, TaskState state, int number, TaskType type, Guid userId, Guid repositoryId)
    {
        Title = title;
        Description = description;
        State = state;
        Number = number;
        Type = type;
        UserId = userId;
        RepositoryId = repositoryId;
    }
    
    protected Task(string title, string description, TaskState state, int number, TaskType type,
        Guid userId, Guid repositoryId, List<RepositoryMember> assignees, List<Label> labels,  Guid? milestoneId)
    {
        Title = title;
        Description = description;
        State = state;
        Number = number;
        Type = type;
        UserId = userId;
        RepositoryId = repositoryId;
        MilestoneId = milestoneId;
        Assignees = assignees;
        Labels = labels;
    }
    
    

}