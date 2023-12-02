using Application.Shared;
using Domain.Milestones;

namespace Application.Milestones.Queries.FindRepositoryMilestones;

public record FindRepositoryMilestonesQuery(Guid UserId, Guid RepositoryId): IQuery<List<Milestone>>;