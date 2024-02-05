using Application.Shared;
using Domain.Milestones;

namespace Application.Milestones.Queries.FindCompletionPercentageOfMilestone;

public record FindCompletionPercentageOfMilestoneQuery(Guid MilestoneId): IQuery<double>;