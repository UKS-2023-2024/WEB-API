using System.Data.Entity.Infrastructure.Design;
using System.Runtime.InteropServices.JavaScript;
using Domain.Repositories;

namespace Domain.Milestones;

public class Milestone
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public DateOnly? DueDate { get; private set; }
    public string Description { get; private set; }
    public Guid RepositoryId { get; private set; }
    public Repository? Repository { get; private set; }
    public bool Closed { get; private set; }
    public List<Domain.Tasks.Task> Tasks { get; private set; }

    private Milestone(string title, string description, DateOnly? dueDate, Guid repositoryId)
    {
        Title = title;
        Description = description;
        RepositoryId = repositoryId;
        DueDate = dueDate;
        Closed = false;
        Tasks = new();
    }

    public static Milestone Create(string title, string description, DateOnly? dueDate, Guid repositoryId)
    {
        return new Milestone(title, description, dueDate, repositoryId);
    }

    public static Milestone Create(Guid id, string title, string description, DateOnly? dueDate, Guid repositoryId)
    {
        Milestone milestone = new Milestone(title, description, dueDate, repositoryId);
        milestone.Id = id;
        return milestone;
    }

    public static Milestone Update(Milestone milestone, string title, string description, DateOnly dueDate)
    {
        milestone.Description = description;
        milestone.DueDate = dueDate;
        milestone.Title = title;
        return milestone;
    }

    public static Milestone Close(Milestone milestone)
    {
        milestone.Closed = true;
        return milestone;
    }

    public static Milestone Reopen(Milestone milestone)
    {
        milestone.Closed = false;
        return milestone;
    }
}