using Domain.Exceptions;

namespace Domain.Repositories.Exceptions;

public class RepositoryMemberCantBeDeletedException:BaseException
{
    public RepositoryMemberCantBeDeletedException() : base("User cant be deleted!")
    {
    }
}