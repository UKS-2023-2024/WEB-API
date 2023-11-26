using Domain.Exceptions;

namespace Domain.Branches.Exceptions;

public class BranchIsAlreadyDefaultException : BaseException
{
    public BranchIsAlreadyDefaultException() : base("Branch is already default!")
    {
    }
}