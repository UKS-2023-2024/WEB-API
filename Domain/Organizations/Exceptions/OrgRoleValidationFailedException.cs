using Domain.Exceptions;

namespace Domain.Organizations.Exceptions;

public class OrgRoleValidationFailedException: BaseException
{
    public OrgRoleValidationFailedException() : base("Validation failed when creating organization role!")
    {
    }
}