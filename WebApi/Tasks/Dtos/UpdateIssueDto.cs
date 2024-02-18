using Domain.Tasks.Enums;

namespace WEB_API.Tasks.Dtos;

public class UpdateIssueDto
{
    public Guid Id { get; set; }

    public string Title { get;  set; }
    public string? Description { get;  set; }
    public Guid RepositoryId { get;  set; }
    public List<string>? AssigneesIds { get;  set; }
    public List<string>? LabelsIds { get;  set; }
    public Guid? MilestoneId { get;  set; }
    public TaskState State { get; set; }
    public int Number { get; set; }
}