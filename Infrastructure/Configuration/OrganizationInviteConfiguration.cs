using Domain.Organizations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class OrganizationInviteConfiguration: IEntityTypeConfiguration<OrganizationInvite>
{
    public void Configure(EntityTypeBuilder<OrganizationInvite> builder)
    {
        builder.HasKey(o => new { o.OrganizationId, o.MemberId, o.Token });
        builder
            .HasOne(i => i.OrganizationMember)
            .WithMany(mem => mem.Invites)
            .HasForeignKey(i => new { i.OrganizationId, i.MemberId });
    }
}