using Domain.Exceptions;

namespace Domain.Repositories.Exceptions;

public class RepositoryNotStarredException:BaseException
{
    public RepositoryNotStarredException() : base("Repository not starred!")
    {
    }
}