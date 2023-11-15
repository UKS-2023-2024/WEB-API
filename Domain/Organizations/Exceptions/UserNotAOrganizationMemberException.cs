using Domain.Exceptions;

namespace Domain.Organizations.Exceptions;

public class UserNotAOrganizationMemberException:BaseException
{
    public UserNotAOrganizationMemberException() : base("User you want to add is not a member of organization!")
    {
    }
}