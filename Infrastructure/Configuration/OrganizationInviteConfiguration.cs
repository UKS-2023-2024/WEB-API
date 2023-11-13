using Domain.Organizations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class OrganizationInviteConfiguration: IEntityTypeConfiguration<OrganizationInvite>
{
    public void Configure(EntityTypeBuilder<OrganizationInvite> builder)
    {
        builder.HasKey(o => o.Id);
        
        builder
            .HasIndex(o => new { o.OrganizationId, MemberId = o.UserId })
            .IsUnique();
        
        builder.HasOne(o => o.Organization)
            .WithMany(o => o.PendingInvites)
            .HasForeignKey(o => o.OrganizationId);

        builder.HasOne(o => o.User)
            .WithMany(u => u.Invites)
            .HasForeignKey(o => o.UserId);
    }
}