using System.ComponentModel.DataAnnotations;

namespace WEB_API.Repositories.Dtos
{
    public class RepositoryDto
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsPrivate { get; set; }
        public Guid OrganizationId { get; set; }

    }
}
