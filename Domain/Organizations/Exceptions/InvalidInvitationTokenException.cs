using Domain.Exceptions;

namespace Domain.Organizations.Exceptions;

public class InvalidInvitationTokenException: BaseException
{
    public InvalidInvitationTokenException() : base("Invalid invitation token exception!")
    {
    }
}