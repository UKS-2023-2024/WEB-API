using Domain.Organizations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class OrganizationRoleConfiguration: IEntityTypeConfiguration<OrganizationRole>
{
    public void Configure(EntityTypeBuilder<OrganizationRole> builder)
    {
        builder.HasKey(r => r.Name);
        
        builder
            .HasMany(o => o.Permissions)
            .WithOne(p => p.Role)
            .HasForeignKey(p => p.RoleName);
        
        //Roles
        builder
            .HasData(OrganizationRole.Owner(), OrganizationRole.Member());
    }
}