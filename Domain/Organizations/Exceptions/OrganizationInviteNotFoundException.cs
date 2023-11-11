using Domain.Exceptions;

namespace Domain.Organizations.Exceptions;

public class OrganizationInviteNotFoundException: BaseException
{
    public OrganizationInviteNotFoundException() : base("Organization invite not found!")
    {
    }
}