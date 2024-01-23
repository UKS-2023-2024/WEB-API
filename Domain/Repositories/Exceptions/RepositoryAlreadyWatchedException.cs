using Domain.Exceptions;

namespace Domain.Repositories.Exceptions;

public class RepositoryAlreadyWatchedException:BaseException
{
    public RepositoryAlreadyWatchedException() : base("Repository already watched!")
    {
    }
}