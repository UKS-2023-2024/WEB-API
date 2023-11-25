using Domain.Exceptions;

namespace Domain.Repositories.Exceptions;

public class MemberHasNoPrivilegeException : BaseException
{
    public MemberHasNoPrivilegeException() : base("You have no privileges for this operation")
    {
    }
}