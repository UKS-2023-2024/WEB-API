using Domain.Shared.Interfaces;

namespace Domain.Tasks.Interfaces;

public interface IPullRequestRepository : IBaseRepository<PullRequest>
{
    Task<PullRequest?> FindByIdAndRepositoryId(Guid repositoryId, Guid pullRequestId);
    Task<List<PullRequest>> FindAllByRepositoryId(Guid repositoryId);
    Task<PullRequest?> FindByBranchesAndRepository(Guid repositoryId, Guid fromBranchId, Guid toBranchId);
}