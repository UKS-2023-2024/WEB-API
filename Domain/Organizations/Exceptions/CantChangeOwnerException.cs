using Domain.Exceptions;

namespace Domain.Organizations.Exceptions;

public class CantChangeOwnerException : BaseException
{
    public CantChangeOwnerException() : base("Can't change owner")
    {
    }
}