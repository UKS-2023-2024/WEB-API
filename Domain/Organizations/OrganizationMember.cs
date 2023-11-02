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

    private OrganizationMember(User member, Guid memberId, Organization organization, Guid organizationId)
    {
        Member = member;
        MemberId = memberId;
        Organization = organization;
        OrganizationId = organizationId;
    }

    public static OrganizationMember Create(User member, Organization organization)
    {
        return new OrganizationMember(member, member.Id, organization, organization.Id);
    }
}