using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class RepositoryInviteConfiguration: IEntityTypeConfiguration<RepositoryInvite>
{
    public void Configure(EntityTypeBuilder<RepositoryInvite> builder)
    {
        builder.HasKey(r => r.Id);
        
        builder
            .HasIndex(r => new { r.RepositoryId, MemberId = r.UserId })
            .IsUnique();
        
        builder.HasOne(ri => ri.Repository)
            .WithMany(r => r.PendingInvites)
            .HasForeignKey(ri => ri.RepositoryId);

        builder.HasOne(r => r.User)
            .WithMany(u => u.RepositoryInvites)
            .HasForeignKey(r => r.UserId);
    }
}