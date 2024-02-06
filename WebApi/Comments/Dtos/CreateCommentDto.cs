namespace WEB_API.Comments.Dtos;

public class CreateCommentDto
{
    public string TaskId { get; set; }
    public string Content { get; set; }

    public CreateCommentDto(string taskId, string content)
    {
        TaskId = taskId;
        Content = content;
    }
}