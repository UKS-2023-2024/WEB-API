using Domain.Exceptions;

namespace Domain.Organizations.Exceptions;

public class CantAccessOrganizationMembers : BaseException
{
    public CantAccessOrganizationMembers() : base("You cant access this organization!")
    {
    }
}