using Domain.Exceptions;

namespace Domain.Organizations.Exceptions;

public class InvitationExpiredException: BaseException
{
    public InvitationExpiredException() : base("Invitation expired!")
    {
    }
}