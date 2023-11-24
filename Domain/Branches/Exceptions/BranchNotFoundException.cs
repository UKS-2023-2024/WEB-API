using Domain.Exceptions;

namespace Domain.Branches.Exceptions;

public class BranchNotFoundException : BaseException
{
    public BranchNotFoundException() : base("Branch not found!")
    {
    }
}