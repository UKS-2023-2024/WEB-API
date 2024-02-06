using Domain.Auth;
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

    public Comment()
    {
    }

    public Comment(Guid creatorId, DateTime createdAt, string content, Guid taskId)
    {
        CreatorId = creatorId;
        CreatedAt = createdAt;
        Content = content;
        TaskId = taskId;
    }

    public static Comment Create(Guid creatorId, DateTime createdAt, string content, Guid taskId)
    {
        return new Comment(creatorId, createdAt, content, taskId);
    }
}