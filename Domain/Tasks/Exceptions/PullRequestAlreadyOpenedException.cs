using Domain.Exceptions;

namespace Domain.Tasks.Exceptions;

public class PullRequestAlreadyOpenedException:BaseException
{
    public PullRequestAlreadyOpenedException() : base("Pull request already opened!")
    {
    }
}