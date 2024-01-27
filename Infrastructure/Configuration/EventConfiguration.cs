using Domain.Tasks;
using Domain.Tasks.Enums;
using Domain.Tasks.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration;

public class EventConfiguration: IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.HasDiscriminator(t => t.EventType)
            .HasValue<AssignEvent>(EventType.ISSUE_ASSIGNED)
            .HasValue<UnassignEvent>(EventType.ISSUE_UNASSIGNED)
            .HasValue<Event>(EventType.OPENED)
            .HasValue<AssignMilestoneEvent>(EventType.MILESTONE_ASSIGNED);
        // .HasValue<Event>(EventType.MILESTONE_UNASSIGNED);
    }
}