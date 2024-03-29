﻿using Domain.Shared.Interfaces;

namespace Domain.Tasks.Interfaces;

public interface IPullRequestRepository : IBaseRepository<PullRequest>
{
    Task<PullRequest?> FindByIdAndRepositoryId(Guid repositoryId, Guid pullRequestId);
    Task<List<PullRequest>> FindAllByRepositoryId(Guid repositoryId);
    Task<PullRequest?> FindOpenByBranchesAndRepository(Guid repositoryId, Guid fromBranchId, Guid toBranchId);
    Task<List<PullRequest>> FindAllAssignedWithLabelInRepository(Label label, Guid repositoryId);
    Task<PullRequest?> FindByNumber(Guid repositoryId, int number);
}