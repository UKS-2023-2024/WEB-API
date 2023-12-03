using Domain.Shared.Interfaces;

namespace Domain.Milestones.Interfaces;

public interface IMilestoneRepository : IBaseRepository<Milestone>
{
    Task<List<Milestone>> FindActiveRepositoryMilestones(Guid repositoryId);
    Task<List<Milestone>> FindClosedRepositoryMilestones(Guid repositoryId);
}