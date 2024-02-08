using Domain.Exceptions;

namespace Domain.Tasks.Exceptions;

public class PullRequestMergedException: BaseException
{
    public PullRequestMergedException(string message) : base(message)
    {
    }
}