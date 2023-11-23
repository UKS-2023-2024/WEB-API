using Application.Shared;
using Domain.Milestones;

namespace Application.Milestones.Queries.FindRepositoryMilestones;

public record FindRepositoryMilestonesCommand(Guid userId, Guid RepositoryId): IQuery<List<Milestone>>;