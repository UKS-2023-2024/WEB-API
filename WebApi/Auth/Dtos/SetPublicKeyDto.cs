using System.ComponentModel.DataAnnotations;

namespace WEB_API.Auth.Dtos;

public class SetPublicKeyDto
{
    [Required]
    public string pk { get; set; }
}