using System.ComponentModel.DataAnnotations;
using Domain.Tasks.Enums;

namespace WEB_API.Tasks.Dtos;

public class CreatePullRequestDto
{
    public string? Id { get; set; }

    [Required]
    public string Title { get;  set; }
    public string? Description { get;  set; }
    [Required]
    public Guid RepositoryId { get;  set; }

    public List<Guid>? AssigneesIds { get; set; } = new();
    public List<Guid>? LabelsIds { get; set; } = new();
    public List<Guid>? IssueIds { get; set; } = new();
    public Guid? MilestoneId { get;  set; }
    public Guid FromBranchId { get; set; }
    public Guid ToBranchId { get; set; }
}