using Domain.Exceptions;

namespace Domain.Tasks.Exceptions;

public class PullRequestNotFoundException:BaseException
{
    public PullRequestNotFoundException() : base("Pull request not found")
    {
    }
}