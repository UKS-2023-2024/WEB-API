using Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class RemoveIssueFromPullRequestConfiguration: IEntityTypeConfiguration<RemoveIssueFromPullRequestEvent>
{
    public void Configure(EntityTypeBuilder<RemoveIssueFromPullRequestEvent> builder)
    {
        builder.HasOne(a => a.Issue)
            .WithMany(r => r.RemovePullRequestEvents)
            .HasForeignKey(a => a.IssueId);
    }
}