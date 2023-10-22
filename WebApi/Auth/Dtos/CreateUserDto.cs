using System.ComponentModel.DataAnnotations;

namespace WEB_API.Auth.Dtos;


public class CreateUserDto
{
    
    [Required]
    public string Email { get; set; }
    [Required]
    public string FullName { get; set; }
}