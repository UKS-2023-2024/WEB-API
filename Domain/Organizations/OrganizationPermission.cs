namespace Domain.Organizations;

public class OrganizationPermission
{
    public string Value { get; private set; }
    public string? Description { get; private set; }

    public List<OrganizationRolePermission> Roles { get; private set; } = new ();

    public OrganizationPermission(string value, string? description = "")
    {
        Value = value;
        Description = description;
    }

    static OrganizationPermission Create(string value)
    {
        return new OrganizationPermission(value);
    }
    
}