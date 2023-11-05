using Domain.Exceptions;

namespace Domain.Organizations.Exceptions;

public class OrganizationNotFoundException : BaseException
{
    public OrganizationNotFoundException() : base("Organization not found!")
    {
    }
}