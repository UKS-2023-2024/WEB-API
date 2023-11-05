using Domain.Auth;
using Domain.Auth.Enums;

namespace WEB_API.Organization.Dtos;

public class UserDto
{
    public Guid Id { get; set; }
    public string PrimaryEmail { get; set; }
    public string FullName { get; set; }
    public string Username { get; set; }
    public UserRole Role { get; set; }

  
}