using Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class UnassignEventConfiguration: IEntityTypeConfiguration<UnassignEvent>
{
    public void Configure(EntityTypeBuilder<UnassignEvent> builder)
    {
        builder.HasOne(a => a.Assignee)
            .WithMany(r => r.UnassignEvents)
            .HasForeignKey(a => a.AssigneeId);
    }
}