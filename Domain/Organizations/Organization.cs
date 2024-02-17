using Domain.Auth;
using Domain.Organizations.Exceptions;
using Domain.Repositories;

namespace Domain.Organizations;

public class Organization
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string ContactEmail { get; private set; }
    private List<OrganizationMember> _members = new();
    public IReadOnlyList<OrganizationMember> Members => new List<OrganizationMember>(_members);
    public string? Description { get; private set; }
    public string? Url { get; private set; }
    public string? Location { get; private set; }
    public List<OrganizationInvite> PendingInvites { get; private set; }
    public List<Repository> Repositories { get; private set; }

    public int? memberTeamId { get; private set; }
    public DateTime? CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private Organization()
    {
    }

    private Organization(string name, string contactEmail, List<User> pendingMembers,User creator)
    {
        Name = name;
        ContactEmail = contactEmail;
        _members = new List<OrganizationMember>();
        PendingInvites = pendingMembers
            .Select(mem => OrganizationInvite.Create(mem.Id, Id))
            .ToList();
        var member = OrganizationMember.Create(creator, this, OrganizationMemberRole.OWNER);
        _members.Add(member);
    }

    public static Organization Create(string name, string contactEmail, List<User> pendingMembers,User creator)
    {
        var newOrganization = new Organization(name, contactEmail, pendingMembers,creator);
        return newOrganization;
    }

    public static Organization Create(Guid id, string name, string contactEmail, List<User> pendingMembers,User creator)
    {
        Organization newOrganization = new Organization(name, contactEmail, pendingMembers,creator);
        newOrganization.Id = id;
        return newOrganization;
    }
    
    public void Created()
    {
        CreatedAt = DateTime.Now.ToUniversalTime();
    }
    public void Updated()
    {
        UpdatedAt = DateTime.Now.ToUniversalTime();
    }
    
    public OrganizationMember AddMember(User user)
    {
        var member = _members.FirstOrDefault(m => m.MemberId == user.Id);
        if (member is not null)
        {
            member.ActivateMemberAgain();
            member.SetRole(OrganizationMemberRole.MEMBER);
            return member;
        }
        member = OrganizationMember.Create(user, this, OrganizationMemberRole.MEMBER);
        _members.Add(member);
        return member;
    }
    
    public void RemoveMember(OrganizationMember organizationMember)
    {
        var member = _members.FirstOrDefault(m => m.MemberId == organizationMember.MemberId);
        if (member is null)
            throw new OrganizationMemberNotFoundException();
        member.Delete();
    }

    public static void ThrowIfDoesntExist(Organization? organization)
    {
        if (organization is null) throw new OrganizationNotFoundException();
    }

    public void SetMemberTeamId(int teamId)
    {
        memberTeamId = teamId;
    }
}