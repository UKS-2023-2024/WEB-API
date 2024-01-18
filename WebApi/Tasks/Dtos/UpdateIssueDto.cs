using Domain.Tasks.Enums;

namespace WEB_API.Tasks.Dtos;

public class UpdateIssueDto
{
    public string Id { get; set; }

    public string Title { get;  set; }
    public string? Description { get;  set; }
    public string RepositoryId { get;  set; }
    public List<string>? AssigneesIds { get;  set; }
    public List<string>? LabelsIds { get;  set; }
    public string? MilestoneId { get;  set; }
    public TaskState State { get; set; }
    public int Number { get; set; }
}