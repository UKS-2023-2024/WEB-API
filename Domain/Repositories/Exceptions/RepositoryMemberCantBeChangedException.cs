using Domain.Exceptions;

namespace Domain.Repositories.Exceptions;

public class RepositoryMemberCantBeChangedException : BaseException
{
    public RepositoryMemberCantBeChangedException() : base("User cant be changed!")
    {
    }
}