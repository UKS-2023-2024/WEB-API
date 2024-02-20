using Domain.Exceptions;

namespace Domain.Tasks.Exceptions;

public class PullRequestDoesNotHaveMilestoneException:BaseException
{
    public PullRequestDoesNotHaveMilestoneException() : base("Pull request does not have milestone!")
    {
    }
}