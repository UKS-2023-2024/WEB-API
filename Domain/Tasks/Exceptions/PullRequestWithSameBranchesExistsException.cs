using Domain.Exceptions;

namespace Domain.Tasks.Exceptions;

public class PullRequestWithSameBranchesExistsException: BaseException
{
    public PullRequestWithSameBranchesExistsException() : base("Pull request with this branches exists!")
    {
    }
}