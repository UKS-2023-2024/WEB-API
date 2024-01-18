using Domain.Repositories;

namespace Domain.Tasks;

public class Label
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public string Color { get; private set; }
    public List<Task> Tasks { get; private set; }
    public Guid RepositoryId { get; private set; }
    public Repository? Repository { get; private set; }

    public Label(string title, string description, string color, Guid repositoryId)
    {
        Title = title;
        Description = description;
        Color = color;
        RepositoryId = repositoryId;
    }
}