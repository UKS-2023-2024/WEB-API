using Application.Shared;
using Domain.Milestones;

namespace Application.Milestones.Queries.FindRepositoryClosedMilestones;

public record FindRepositoryClosedMilestonesQuery(Guid UserId, Guid RepositoryId): IQuery<List<Milestone>>;