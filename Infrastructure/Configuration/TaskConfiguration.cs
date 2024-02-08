
using Domain.Tasks;
using Domain.Tasks.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Infrastructure.Configuration;

public class TaskConfiguration: IEntityTypeConfiguration<Domain.Tasks.Task>
{
    public void Configure(EntityTypeBuilder<Domain.Tasks.Task> builder)
    {
        builder.HasMany(t => t.Labels)
            .WithMany(l => l.Tasks);

        builder.HasMany(t => t.Assignees)
            .WithMany(a => a.Tasks);

        builder.HasOne(t => t.Repository)
            .WithMany(r => r.Tasks)
            .HasForeignKey(t => t.RepositoryId);

        builder.HasOne(t => t.Milestone)
            .WithMany(m => m.Tasks)
            .HasForeignKey(t => t.MilestoneId);

        builder.HasDiscriminator(t => t.Type)
            .HasValue<Issue>(TaskType.ISSUE)
            .HasValue<PullRequest>(TaskType.PULL_REQUEST);

        builder.HasMany(t => t.Events)
            .WithOne(e => e.Task)
            .HasForeignKey(e => e.TaskId);
    }
}