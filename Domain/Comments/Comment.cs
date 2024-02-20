using Domain.Auth;
using Domain.Reactions;
using Microsoft.AspNetCore.Http.HttpResults;
using Task = Domain.Tasks.Task;

namespace Domain.Comments;

public class Comment
{
    public Guid Id { get; private set; }
    public Guid CreatorId { get; private set; }
    public User Creator { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public string Content { get; private set; }
    public Guid TaskId { get; private set; }
    public Task Task { get; private set; }
    public List<Reaction> Reactions { get; private set; } = new();
    public Guid? ParentId { get; private set; }

    public Comment()
    {
    }

    public Comment(Guid creatorId, DateTime createdAt, string content, Guid taskId, Guid? parentId)
    {
        CreatorId = creatorId;
        CreatedAt = createdAt;
        Content = content;
        TaskId = taskId;
        ParentId = parentId;
    }

    public static Comment Create(Guid creatorId, DateTime createdAt, string content, Guid taskId, Guid? parentId)
    {
        return new Comment(creatorId, createdAt, content, taskId, parentId);
    }
}