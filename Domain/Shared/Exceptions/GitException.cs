using Domain.Exceptions;

namespace Domain.Shared.Exceptions;

public class GitException: BaseException
{
    public GitException(string message) : base(message)
    {
    }
}