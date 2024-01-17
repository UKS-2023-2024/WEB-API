using Domain.Exceptions;

namespace Domain.Organizations.Exceptions;

public class CantRemoveOrganizationOwnerException : BaseException
{
    public CantRemoveOrganizationOwnerException() : base("Member can't be removed because he is owner!")
    {
    }
}