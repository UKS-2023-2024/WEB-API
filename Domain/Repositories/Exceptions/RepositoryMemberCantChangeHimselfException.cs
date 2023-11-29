using Domain.Exceptions;

namespace Domain.Repositories.Exceptions;

public class RepositoryMemberCantChangeHimselfException : BaseException
{
    public RepositoryMemberCantChangeHimselfException() : base("You can't change yourself in repository")
    {
    }
}