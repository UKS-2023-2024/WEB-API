namespace WEB_API.Organization.Presenters;

public class OrganizationPresenter
{
    public string Name { get; set; }
    public string ContactEmail { get; set; }

    // public List<string> PendingMembers { get; set; }

    public OrganizationPresenter(Domain.Organizations.Organization organization)
    {
        Name = organization.Name;
        ContactEmail = organization.ContactEmail;
    }

    public static List<OrganizationPresenter> MapOrganizationsToOrganizationPresenters(
        List<Domain.Organizations.Organization> organizations)
    {
        return organizations.Select(org => new OrganizationPresenter(org)).ToList();
    }
}