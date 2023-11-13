using Domain.Auth;
using Domain.Organizations.Exceptions;

namespace Domain.Organizations;

public class OrganizationMember
{
    public Guid Id { get; private set; }
    public User Member { get; private set; } = null;
    public Guid MemberId { get; private set; }
    public Organization Organization { get; private set; } = null;
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

    private OrganizationMember(Guid userId, Guid organizationId, OrganizationRole role)
    {
        MemberId = userId;
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

    public static OrganizationMember Create(Guid userId, Guid organizationId, OrganizationRole role)
    {
        return new OrganizationMember(userId, organizationId, role);
    }

    public static void ThrowIfDoesntExist(OrganizationMember? member)
    {
        if (member is null) throw new OrganizationMemberNotFoundException();
    }
    
}