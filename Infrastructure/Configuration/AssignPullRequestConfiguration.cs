using Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class AssignPullRequestConfiguration: IEntityTypeConfiguration<AssignPullRequestEvent>
{
    public void Configure(EntityTypeBuilder<AssignPullRequestEvent> builder)
    {
        builder.HasOne(a => a.Assignee)
            .WithMany(r => r.AssignPullRequestEvents)
            .HasForeignKey(a => a.AssigneeId);
    }
}