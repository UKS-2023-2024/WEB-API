using System.ComponentModel.DataAnnotations;

namespace WEB_API.Milestones.Dtos;

public class MilestoneDto
{
    public string Id { get; set; }
    [Required] public string RepositoryId { get; set; }
    [Required] public string Title { get; set; }
    [Required] public string Description { get; set; }
    public DateOnly DueDate { get; set; }
}