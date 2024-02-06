using Domain.Tasks;
using Domain.Tasks.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Shared.Repositories;

namespace Infrastructure.Tasks;

public class PullRequestRepository: BaseRepository<PullRequest>, IPullRequestRepository
{
    private readonly MainDbContext _context;
    public PullRequestRepository(MainDbContext context) : base(context)
    {
        _context = context;
    }
}