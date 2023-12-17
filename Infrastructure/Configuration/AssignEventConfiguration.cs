using Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class AssignEventConfiguration: IEntityTypeConfiguration<AssignEvent>
{
    public void Configure(EntityTypeBuilder<AssignEvent> builder)
    {
        builder.HasOne(a => a.Assignee)
            .WithMany(r => r.AssignEvents)
            .HasForeignKey(a => a.AssigneeId);
    }
}