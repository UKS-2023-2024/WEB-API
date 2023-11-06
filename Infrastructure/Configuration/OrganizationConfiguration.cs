using Domain.Organizations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class OrganizationConfiguration: IEntityTypeConfiguration<Organization>
{
    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        builder
            .HasMany(o => o.Members)
            .WithOne(m => m.Organization)
            .HasForeignKey(o => o.OrganizationId);

        builder
            .HasMany(o => o.PendingMembers)
            .WithMany(u => u.PendingOrganizations);
    }
}