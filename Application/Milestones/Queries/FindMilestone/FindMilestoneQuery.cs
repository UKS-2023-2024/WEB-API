using Application.Shared;
using Domain.Milestones;
using Domain.Milestones.Exceptions;

namespace Application.Milestones.Queries.FindMilestone;

public record FindMilestoneQuery(Guid MilestoneId): IQuery<Milestone>;