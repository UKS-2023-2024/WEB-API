using Domain.Organizations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class OrganizationPermissionConfiguration: IEntityTypeConfiguration<OrganizationPermission>
{
    public void Configure(EntityTypeBuilder<OrganizationPermission> builder)
    {
        builder
            .HasKey(p => p.Value);

        builder
            .HasMany(o => o.Roles)
            .WithOne(o => o.Permission)
            .HasForeignKey(o => o.PermissionName);
        //Seeding
        //Permissions
        builder
            .HasData(GetSeedPermissions());
    }
    private List<OrganizationPermission> GetSeedPermissions()
        => new() { new("owner"), new("admin"), new("manager"), new("read_only") };
}