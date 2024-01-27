using Domain.Exceptions;

namespace Domain.Organizations.Exceptions;

public class CantChangeOrganizationOwnerException : BaseException
{
    public CantChangeOrganizationOwnerException() : base("Can't change organization owner")
    {
    }
}