using Domain.Exceptions;

namespace Domain.Auth.Exceptions;

public class UserNotFoundException : BaseException
{
    public UserNotFoundException() : base("User not found!")
    {
    }
}