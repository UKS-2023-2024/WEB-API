using System.ComponentModel.DataAnnotations;

namespace WEB_API.Organization.Dtos;

public class OrganizationDto
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string ContactEmail { get; set; }

    public List<string> PendingMembers { get; set; }
}