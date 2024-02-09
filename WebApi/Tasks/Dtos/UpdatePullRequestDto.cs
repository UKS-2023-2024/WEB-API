using Domain.Tasks.Enums;

namespace WEB_API.Tasks.Dtos;

public class UpdatePullRequestDto
{
    public Guid Id { get; set; }
    public List<Guid>? IssueIds { get;  set; }
}