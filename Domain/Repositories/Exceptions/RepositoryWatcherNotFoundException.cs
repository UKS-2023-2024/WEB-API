using Domain.Exceptions;

namespace Domain.Repositories.Exceptions;

public class RepositoryWatcherNotFoundException : BaseException
{
    public RepositoryWatcherNotFoundException() : base("Repository watcher not found!")
    {
    }
}