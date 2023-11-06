using Domain.Organizations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class OrganizationRolePermissionConfiguration: IEntityTypeConfiguration<OrganizationRolePermission>
{
    public void Configure(EntityTypeBuilder<OrganizationRolePermission> builder)
    {
        builder
            .HasKey(o =>
                new {
                    o.RoleName, o.PermissionName
                });
        //Roles
        //Permission-Role connection
        builder
            .HasData(GetSeedPermissionRole());
    }
    private List<OrganizationRolePermission> GetSeedPermissionRole()
        => new()
        {
            new("OWNER", "owner"),
            new("OWNER", "admin"),
            new("OWNER", "manager"),
            new("OWNER", "read_only"),
            new("MEMBER", "manager"),
            new("MEMBER", "read_only"),
        };
}