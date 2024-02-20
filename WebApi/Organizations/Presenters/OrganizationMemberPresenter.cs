using Domain.Organizations;

namespace WEB_API.Organizations.Presenters;

public class OrganizationMemberPresenter
{
    public OrganizationMemberRole Role { get; private set; }
    public Guid MemberId { get; private set; }
    public string PrimaryEmail { get; private set; }
    public string Username { get; private set; }
    
    public OrganizationMemberPresenter(OrganizationMember organizationMember)
    {
        MemberId = organizationMember.MemberId;
        Role = organizationMember.Role;
        PrimaryEmail = organizationMember.Member.PrimaryEmail;
        Username = organizationMember.Member.Username;
    }

    public static IEnumerable<OrganizationMemberPresenter> MapOrganizationMembersToOrganizationMemberPresenters(
        IEnumerable<OrganizationMember> organizationsMembers)
    {
        return organizationsMembers.Select(org => new OrganizationMemberPresenter(org));
    }
}