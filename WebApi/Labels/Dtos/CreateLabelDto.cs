using System.ComponentModel.DataAnnotations;

namespace WEB_API.Labels.Dtos;

public class CreateLabelDto
{
    [Required]
    public string Title { get; set; }
    public string Description { get; set; }
    public string RepositoryId { get; set; }
    public string? TaskId { get; set; }
    public string Color { get; set; }
    public bool IsDefaultLabel { get; set; }
}