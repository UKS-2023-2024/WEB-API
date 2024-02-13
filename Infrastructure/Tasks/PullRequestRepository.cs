using Domain.Tasks;
using Domain.Tasks.Enums;
using Domain.Tasks.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace Infrastructure.Tasks;

public class PullRequestRepository: BaseRepository<PullRequest>, IPullRequestRepository
{
    private readonly MainDbContext _context;
    public PullRequestRepository(MainDbContext context) : base(context)
    {
        _context = context;
    }

    public Task<PullRequest?> FindByIdAndRepositoryId(Guid repositoryId, Guid pullRequestId)
    {
       return _context.PullRequests.Include(pr => pr.Events)
           .Include(pr=> pr.FromBranch)
           .Include(pr=> pr.ToBranch)
           .Include(pr => pr.Events)
           .ThenInclude(e=>e.Creator)
            .FirstOrDefaultAsync(pr => pr.Id.Equals(pullRequestId) && pr.RepositoryId.Equals(repositoryId));
    }

    public Task<List<PullRequest>> FindAllByRepositoryId(Guid repositoryId)
    {
        return Task.FromResult(_context.PullRequests.Include(pr => pr.Events)
            .Include(pr => pr.FromBranch)
            .Include(pr => pr.ToBranch)
            .Where(pr => pr.RepositoryId.Equals(repositoryId))
            .ToList());
    }

    public Task<PullRequest?> FindOpenByBranchesAndRepository(Guid repositoryId, Guid fromBranchId, Guid toBranchId)
    {
        return _context.PullRequests
            .FirstOrDefaultAsync(pr => pr.RepositoryId.Equals(repositoryId) 
                                       && pr.FromBranchId.Equals(fromBranchId) 
                                       && pr.ToBranchId.Equals(toBranchId)
                                       && pr.State == TaskState.OPEN);
    }

    public override PullRequest? Find(Guid id)
    {
        return _context.PullRequests.Include(pr => pr.Events)
          .Include(pr => pr.FromBranch)
          .Include(pr => pr.ToBranch)
          .Include(pr => pr.Events)
          .ThenInclude(e => e.Creator)
          .Include(pr => pr.Issues)
          .Include(pr => pr.Milestone)
          .Include(pr => pr.Assignees)
          .ThenInclude(mem => mem.Member)
          .Include(pr => pr.Labels)
          .FirstOrDefault(pr => pr.Id.Equals(id));
    }
}