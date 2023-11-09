using Domain.Exceptions;

namespace Domain.Repositories.Exceptions;

public class RepositoryAlreadyStarredException:BaseException
{
    public RepositoryAlreadyStarredException() : base("Repository already starred!")
    {
    }
}