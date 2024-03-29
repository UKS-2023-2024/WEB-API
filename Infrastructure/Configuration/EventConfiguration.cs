﻿using Domain.Tasks;
using Domain.Tasks.Enums;
using Domain.Tasks.Exceptions;
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
            .HasValue<AssignMilestoneEvent>(EventType.MILESTONE_ASSIGNED)
            .HasValue<UnassignMilestoneEvent>(EventType.MILESTONE_UNASSIGNED)
            .HasValue<AssignPullRequestEvent>(EventType.PULL_REQUEST_ASSIGNED)
            .HasValue<UnnassignPullRequestEvent>(EventType.PULL_REQUEST_UNASSIGNED)
            .HasValue<AddIssueToPullRequestEvent>(EventType.PULL_REQUEST_ISSUE_ADDED)
            .HasValue<RemoveIssueFromPullRequestEvent>(EventType.PULL_REQUEST_ISSUE_REMOVED)
            .HasValue<Event>(EventType.OPENED)
            .HasValue<CloseEvent>(EventType.CLOSED)
            .HasValue<PullRequestMergedEvent>(EventType.PULL_REQUEST_MERGED)
            .HasValue<AssignLabelEvent>(EventType.LABEL_ASSIGNED)
            .HasValue<UnassignLabelEvent>(EventType.LABEL_UNASSIGNED);
        
        builder.Navigation(e => e.Creator).AutoInclude();
    }
}