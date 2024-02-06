using Domain.Exceptions;

namespace Domain.Branches.Exceptions;

public class CantCreatePullRequestOnSameBranchException: BaseException
{
    public CantCreatePullRequestOnSameBranchException() : base("From and to branches can't be the same!")
    {
    }
}