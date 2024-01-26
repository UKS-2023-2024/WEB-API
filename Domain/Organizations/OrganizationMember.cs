using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Organizations.Exceptions;
using Domain.Repositories.Exceptions;

namespace Domain.Organizations;

public class OrganizationMember
{
    public User Member { get; private set; } = null;
    public Guid MemberId { get; private set; }
    public Organization Organization { get; private set; } = null;
    
    public bool Deleted { get; private set; }
    public Guid OrganizationId { get; private set; }
    public OrganizationMemberRole Role { get; private set; }

    public OrganizationMember()
    {
    }

    private OrganizationMember(User member, Guid memberId, Organization organization, 
        Guid organizationId, OrganizationMemberRole role)
    {
        Member = member;
        MemberId = memberId;
        Organization = organization;
        OrganizationId = organizationId;
        Role = role;
        Deleted = false;
    }

    private OrganizationMember(Guid userId, Guid organizationId, OrganizationMemberRole role)
    {
        MemberId = userId;
        OrganizationId = organizationId;
        Role = role;
    }

    public bool HasRole(OrganizationMemberRole role)
    {
        return Role.Equals(role);
    }

    public static OrganizationMember Create(User member, Organization organization, OrganizationMemberRole role)
    {
        return new OrganizationMember(member, member.Id, organization, organization.Id, role);
    }

    public static OrganizationMember Create(Guid userId, Guid organizationId, OrganizationMemberRole role)
    {
        return new OrganizationMember(userId, organizationId, role);
    }
    
    public void ActivateMemberAgain()
    {
        Deleted = false;
    }

    public void Delete()
    {
        Deleted = true;
    }

    public static void ThrowIfDoesntExist(OrganizationMember? member)
    {
        if (member is null || member.Deleted) throw new OrganizationMemberNotFoundException();
    }
    
    public void ThrowIfNoAdminPrivileges()
    {
        if (Role != OrganizationMemberRole.OWNER && Role != OrganizationMemberRole.MODERATOR) throw new MemberHasNoPrivilegeException();
    }
    
    public void ThrowIfNotOwner()
    {
        if (Role != OrganizationMemberRole.OWNER) throw new MemberHasNoPrivilegeException();
    }
    
    public void ThrowIfSameAs(OrganizationMember organizationMember)
    {
        if (MemberId.Equals(organizationMember.MemberId)) throw new MemberCantChangeHimselfException();
    }
    
    public void SetRole(OrganizationMemberRole role)
    {
        Role = role;
    }

}