using Domain.Exceptions;

namespace Domain.Repositories.Exceptions;

public class MemberCantChangeHimselfException : BaseException
{
    public MemberCantChangeHimselfException() : base("You can't change yourself as member")
    {
    }
}