using System.ComponentModel.DataAnnotations;

namespace WEB_API.Auth.Dtos;

public class RegisterUserDto
{
    [Required] [EmailAddress] public string PrimaryEmail { get; set; }

    [Required] [MinLength(6)] public string Password { get; set; }

    [Required] public string Fullname { get; set; }

    [Required] public string Username { get; set; }

}