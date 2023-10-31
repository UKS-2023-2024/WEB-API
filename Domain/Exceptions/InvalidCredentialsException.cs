namespace Domain.Exceptions;

public class InvalidCredentialsException : BaseException
{
    public InvalidCredentialsException() : base("Invalid credentials!")
    {
    }
}