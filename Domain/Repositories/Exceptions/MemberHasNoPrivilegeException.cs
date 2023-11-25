using Domain.Exceptions;

namespace Domain.Repositories.Exceptions;

public class MemberHasNoPrivilegeException : BaseException
{
    public MemberHasNoPrivilegeException(string message) : base(message)
    {
    }
}