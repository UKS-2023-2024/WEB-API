using Domain.Auth;
using Domain.Repositories;
using Domain.Tasks.Enums;

namespace Domain.Tasks;

public class Issue: Task
{
    
    public Issue(string title, string description, TaskState state, int number, Guid userId, Guid repositoryId) : 
        base(title, description, state, number, TaskType.ISSUE, userId, repositoryId)
    {
        Events.Add(new Event("Opened issue", EventType.OPENED, userId));
    }
    
    public static Issue Create(string title, string description, TaskState state, int number, Repository repository,
        User creator)
    {
        return new Issue(title, description, state, number, creator.Id, repository.Id);
    }

}