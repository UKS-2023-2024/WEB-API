using Domain.Exceptions;

namespace Domain.Auth.Exceptions;

public class UserWithThisEmailExistsException : BaseException
{
    public UserWithThisEmailExistsException() : base("User with this email already exists!")
    {
    }
}