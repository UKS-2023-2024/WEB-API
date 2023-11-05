using Domain.Exceptions;

namespace Domain.Auth.Exceptions;

public class InvalidCredentialsException : BaseException
{
    public InvalidCredentialsException() : base("Invalid credentials!")
    {
    }
}