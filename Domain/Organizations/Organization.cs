using Domain.Auth;

namespace Domain.Organizations;

public class Organization
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string ContactEmail { get; private set; }
    public List<OrganizationMember> Members { get; private set; }
    public string Description { get; private set; }
    public List<User> PendingMembers { get; private set; }
    public string Url { get; private set; }
    public string Location { get; private set; }

    private Organization()
    {
    }

    private Organization(string name, string contactEmail, List<User> pendingMembers)
    {
        Name = name;
        ContactEmail = contactEmail;
        Members = new List<OrganizationMember>();
        PendingMembers = pendingMembers;
    }

    public Organization Create(string name, string contactEmail, List<User> pendingMembers, OrganizationMember creator)
    {
        Organization newOrganization = new Organization(name, contactEmail, pendingMembers);
        newOrganization.Members.Add(creator);
        return newOrganization;
    }
}