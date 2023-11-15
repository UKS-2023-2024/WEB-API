using Domain.Exceptions;

namespace Domain.Repositories.Exceptions;

public class MemberNotOwnerException : BaseException
{
    public MemberNotOwnerException() : base("You cant add member!")
    {
    }
}