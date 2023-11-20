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

    private Milestone(string title, string description, DateOnly? dueDate, Guid repositoryId)
    {
        Title = title;
        Description = description;
        RepositoryId = repositoryId;
        DueDate = dueDate;
    }

    public static Milestone Create(string title, string description, DateOnly? dueDate, Guid repositoryId)
    {
        return new Milestone(title, description, dueDate, repositoryId);
    }
}