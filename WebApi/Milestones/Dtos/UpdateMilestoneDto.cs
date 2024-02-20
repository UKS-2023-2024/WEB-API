using System.ComponentModel.DataAnnotations;

namespace WEB_API.Milestones.Dtos;

public class UpdateMilestoneDto
{
    [Required] public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateOnly DueDate { get; set; }

}