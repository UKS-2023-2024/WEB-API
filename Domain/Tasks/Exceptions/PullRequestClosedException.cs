using Domain.Exceptions;

namespace Domain.Tasks.Exceptions;

public class PullRequestClosedException:BaseException
{
    public PullRequestClosedException(string message) : base(message)
    {
    }
}