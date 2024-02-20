using Domain.Exceptions;

namespace Domain.Repositories.Exceptions;

public class RepositoryInaccessibleException: BaseException
{
    public RepositoryInaccessibleException() : base("You have no rights to this repository!")
    {
    }
}