using Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class PullRequestConfiguration: IEntityTypeConfiguration<PullRequest>
{
    public void Configure(EntityTypeBuilder<PullRequest> builder)
    {
        builder.HasOne(pr => pr.FromBranch)
            .WithMany(b =>  b.FromPullRequests)
            .HasForeignKey(t => t.FromBranchId);
        
        builder.HasOne(pr => pr.ToBranch)
            .WithMany(b =>  b.ToPullRequests)
            .HasForeignKey(t => t.ToBranchId);
        
        builder.HasMany(pr => pr.Issues)
            .WithMany(b =>  b.PullRequests);
    }
}