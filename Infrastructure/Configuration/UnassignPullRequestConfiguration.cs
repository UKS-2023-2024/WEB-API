using Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class UnassignPullRequestConfiguration: IEntityTypeConfiguration<UnnassignPullRequestEvent>
{
    public void Configure(EntityTypeBuilder<UnnassignPullRequestEvent> builder)
    {
        builder.HasOne(a => a.Assignee)
            .WithMany(r => r.UnnassignPullRequestEvents)
            .HasForeignKey(a => a.AssigneeId);
    }
}