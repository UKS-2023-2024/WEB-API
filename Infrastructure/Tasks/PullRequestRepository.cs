﻿using System.Data.Entity;
using Domain.Tasks;
using Domain.Tasks.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Shared.Repositories;
using Task = System.Threading.Tasks.Task;

namespace Infrastructure.Tasks;

public class PullRequestRepository: BaseRepository<PullRequest>, IPullRequestRepository
{
    private readonly MainDbContext _context;
    public PullRequestRepository(MainDbContext context) : base(context)
    {
        _context = context;
    }

    public Task<PullRequest> FindByIdAndRepositoryId(Guid repositoryId, Guid pullRequestId)
    {
       return _context.PullRequests.Include(pr => pr.Events)
           .Include(pr=> pr.FromBranch)
           .Include(pr=> pr.ToBranch)
           .Include(pr => pr.Events)
            .FirstOrDefaultAsync(pr => pr.Id.Equals(pullRequestId) && pr.RepositoryId.Equals(repositoryId));
    }

    public Task<List<PullRequest>> FindAllByRepositoryId(Guid repositoryId)
    {
        return Task.FromResult(_context.PullRequests.Include(pr => pr.Events)
            .Include(pr=> pr.FromBranch)
            .Include(pr=> pr.ToBranch)
            .Where(pr => pr.RepositoryId.Equals(repositoryId)).ToList());
    }
}