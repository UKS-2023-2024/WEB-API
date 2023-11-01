using Domain.Auth;

namespace Domain.Organizations;

public class OrganizationMember
{
    public Guid Id { get; private set; }
    public User Member { get; private set; }
    public Guid MemberId { get; private set; }
    public Organization Organization { get; private set; }
    public Guid OrganizationId { get; private set; }

    public OrganizationMember()
    {
    }
}