using Domain.Auth;

namespace Domain.Organizations;

public class OrganizationMember
{
    public Guid Id { get; private set; }
    public User Member { get; private set; }
    public Guid MemberId { get; private set; }
    public Organization Organization { get; private set; }
    public Guid OrganizationId { get; private set; }
    public OrganizationRole Role { get; private set; }

    public OrganizationMember()
    {
    }

    private OrganizationMember(User member, Guid memberId, Organization organization, 
        Guid organizationId, OrganizationRole role)
    {
        Member = member;
        MemberId = memberId;
        Organization = organization;
        OrganizationId = organizationId;
        Role = role;
    }

    public bool HasRole(OrganizationRole role)
    {
        return Role.Equals(role);
    }

    public static OrganizationMember Create(User member, Organization organization, OrganizationRole role)
    {
        return new OrganizationMember(member, member.Id, organization, organization.Id, role);
    }
    
}