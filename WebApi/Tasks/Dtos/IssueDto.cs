using System.ComponentModel.DataAnnotations;
using Domain.Milestones;
using Domain.Repositories;
using Domain.Tasks;

namespace WEB_API.Tasks.Dtos;

public class IssueDto
{
    [Required]
    public string Title { get;  set; }
    public string Description { get;  set; }
    [Required]
    public string RepositoryId { get;  set; }
    public List<RepositoryMember> Assignees { get;  set; }
    public List<Label> Labels { get;  set; }
    public string? MilestoneId { get;  set; }

}