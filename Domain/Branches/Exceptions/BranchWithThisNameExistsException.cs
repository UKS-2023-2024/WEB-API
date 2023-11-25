using Domain.Exceptions;

namespace Domain.Branches.Exceptions;

public class BranchWithThisNameExistsException : BaseException
{
    public BranchWithThisNameExistsException() : base("Branch with this name already exists!")
    {
    }
}