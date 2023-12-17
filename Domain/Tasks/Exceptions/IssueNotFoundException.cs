using Domain.Exceptions;

namespace Domain.Tasks.Exceptions;

public class IssueNotFoundException: BaseException
{
    public IssueNotFoundException() : base("Issue not found!")
    {
    }
}