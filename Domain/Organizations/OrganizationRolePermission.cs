namespace Domain.Organizations;

public class OrganizationRolePermission
{
    public string RoleName { get; private set; }
    public string PermissionName { get; private set; }
    
    public OrganizationRole Role { get; private set; }
    public OrganizationPermission Permission { get; private set; }


    public OrganizationRolePermission(string roleName, string permissionName)
    {
        RoleName = roleName;
        PermissionName = permissionName;
    }
}