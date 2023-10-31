namespace Domain.Exceptions;

public class UserNotFoundException : BaseException
{
    public UserNotFoundException() : base("User not found!")
    {
    }
}