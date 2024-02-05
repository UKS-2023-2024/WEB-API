using Domain.Milestones;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class MilestoneConfiguration : IEntityTypeConfiguration<Milestone>
{
    public void Configure(EntityTypeBuilder<Milestone> builder)
    {
        builder.HasOne(m => m.Repository)
            .WithMany(r => r.Milestones)
            .HasForeignKey(m => m.RepositoryId);
        
    }
}