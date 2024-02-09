using Domain.Tasks;
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
        try
        {
            return Task.FromResult(_context.PullRequests.Include(pr => pr.Events)
                .Include(pr => pr.FromBranch)
                .Include(pr => pr.ToBranch)
                .Where(pr => pr.RepositoryId.Equals(repositoryId)).ToList());
        } catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return null;
        }
    }

    public Task<PullRequest?> FindByBranchesAndRepository(Guid repositoryId, Guid fromBranchId, Guid toBranchId)
    {
        return _context.PullRequests
            .FirstOrDefaultAsync(pr => pr.RepositoryId.Equals(repositoryId) && pr.FromBranchId.Equals(fromBranchId) && pr.ToBranchId.Equals(toBranchId) );
    }

    public override PullRequest? Find(Guid id)
    {
        return _context.PullRequests.Include(pr => pr.Events)
          .Include(pr => pr.FromBranch)
          .Include(pr => pr.ToBranch)
          .Include(pr => pr.Events)
          .ThenInclude(e => e.Creator)
          .FirstOrDefault(pr => pr.Id.Equals(id));
    }
}