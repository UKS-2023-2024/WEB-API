using Library.Enums;

namespace Library.Auth;

public class User
{
    
    public string Id { get; private set; }
    public string Email { get; private set; }
    public string FullName { get; private set; }
    public UserRole Role { get; private set; }


    public User(string id, string email, string fullName, UserRole role)
    {
        Id = id;
        Email = email;
        FullName = fullName;
        Role = role;
    }
}