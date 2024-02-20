using Domain.Auth;
using System.ComponentModel.DataAnnotations;

namespace WEB_API.Repositories.Dtos
{
    public class UpdateRepositoryDto
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

        public bool IsPrivate { get; set; }

    }
}
