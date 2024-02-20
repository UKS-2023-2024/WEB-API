using Domain.Exceptions;

namespace Domain.Organizations.Exceptions;

public class OrganizationMemberNotFoundException: BaseException
{
    public OrganizationMemberNotFoundException() : base("Organization member not found!")
    {
    }
}