using Domain.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Domain.Tasks;

public class Label
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public string Color { get; private set; }
    public List<Task> Tasks { get; set; } = new();
    public Guid RepositoryId { get; private set; }
    public Repository? Repository { get; private set; }
    public bool IsDefaultLabel { get; private set; }

    public Label(string title, string description, string color, Guid repositoryId, bool isDefaultLabel = false)
    {
        Title = title;
        Description = description;
        Color = color;
        RepositoryId = repositoryId;
        IsDefaultLabel = isDefaultLabel;
    }

    public static Label Create(string title, string description, string color, Guid repositoryId, bool isDefaultLabel = false)
    {
        return new Label(title, description, color, repositoryId, isDefaultLabel);
    }

    public Label Update(string title, string description, string color)
    {
        Title = title;
        Description = description;
        Color = color;
        return this;
    }
}