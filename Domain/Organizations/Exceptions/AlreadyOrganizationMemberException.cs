using Domain.Exceptions;

namespace Domain.Organizations.Exceptions;

public class AlreadyOrganizationMemberException: BaseException
{
    public AlreadyOrganizationMemberException() : base("Provided user is already organization member!")
    {
    }
}