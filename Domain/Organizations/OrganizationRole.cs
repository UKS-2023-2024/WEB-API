using Domain.Organizations.Exceptions;

namespace Domain.Organizations;

public class OrganizationRole
{
    public string Name { get; private set; }
    public string? Description { get; private set; } = "";
    public List<OrganizationRolePermission> Permissions { get; private set; } = new();
    public List<OrganizationMember> Members { get; set; } = new();


    private OrganizationRole(string name, string description, List<OrganizationRolePermission> permissions)
    {
        Name = name;
        Description = description;
        Permissions = permissions;
    }
    
    private OrganizationRole(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public bool IsOwner()
    {
        return Name.Equals("OWNER");
    }

    public bool IsMember()
    {
        return Name.Equals("MEMBER");
    }

    public bool Equals(OrganizationRole? role)
    {
        if (role is null) return false;
        return role.Name.Equals(Name);
    }


    public static OrganizationRole Create(string name, string description)
    {
        if (name is null || name.Trim().Equals("")) 
            throw new OrgRoleValidationFailedException();
        return new OrganizationRole(name, description);
    }
    
    public static OrganizationRole Create(string name, string description, List<OrganizationRolePermission> permissions)
    {
        if (name is null || name.Trim().Equals("")) 
            throw new OrgRoleValidationFailedException();
        return new OrganizationRole(name, description, permissions);
    }

    public static OrganizationRole Member(
      List<OrganizationRolePermission> permissions = null
        ) => 
        new("MEMBER", "Member has all rights except owners", permissions);
    public static OrganizationRole Owner(
        List<OrganizationRolePermission> permissions = null
        ) => 
        new("OWNER", "Has all rights!", permissions);
    
}