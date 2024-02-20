using Domain.Exceptions;

namespace Domain.Repositories.Exceptions;

public class RepositoryNotWatchedException:BaseException
{
    public RepositoryNotWatchedException() : base("Repository not watched!")
    {
    }
}