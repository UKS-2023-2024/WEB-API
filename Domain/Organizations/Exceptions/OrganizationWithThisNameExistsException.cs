using Domain.Exceptions;

namespace Domain.Organizations.Exceptions;

public class OrganizationWithThisNameExistsException : BaseException
{
    public OrganizationWithThisNameExistsException() : base("Organization with this name already exists!")
    {
    }
}