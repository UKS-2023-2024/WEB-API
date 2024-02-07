using Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class AddIssueToPullRequestConfiguration: IEntityTypeConfiguration<AddIssueToPullRequestEvent>
{
    public void Configure(EntityTypeBuilder<AddIssueToPullRequestEvent> builder)
    {
        builder.HasOne(a => a.Issue)
            .WithMany(r => r.AddPullRequestEvents)
            .HasForeignKey(a => a.IssueId);
    }
}