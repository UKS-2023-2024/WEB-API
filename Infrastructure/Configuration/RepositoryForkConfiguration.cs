using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class RepositoryForkConfiguration: IEntityTypeConfiguration<RepositoryFork>
{
    public void Configure(EntityTypeBuilder<RepositoryFork> builder)
    {
        builder.HasKey(member => new { member.SourceRepoId, member.ForkedRepoId });
        builder
            .HasOne(r => r.ForkedRepo)
            .WithOne(w => w.ForkedFrom)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .HasOne(r => r.SourceRepo)
            .WithMany(w => w.RepositoryForks)
            .HasForeignKey(r=>r.SourceRepoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}