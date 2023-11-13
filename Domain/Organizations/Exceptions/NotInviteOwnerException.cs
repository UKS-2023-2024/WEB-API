using Domain.Exceptions;

namespace Domain.Organizations.Exceptions;

public class NotInviteOwnerException: BaseException
{
    public NotInviteOwnerException() : base("Not an invite owner!")
    {
    }
}