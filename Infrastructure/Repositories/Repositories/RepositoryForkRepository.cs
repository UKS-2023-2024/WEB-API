using Domain.Repositories;
using Domain.Repositories.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Shared.Repositories;

namespace Infrastructure.Repositories.Repositories;

public class RepositoryForkRepository: BaseRepository<RepositoryFork>, IRepositoryForkRepository
{
    private readonly MainDbContext _context;
    public RepositoryForkRepository(MainDbContext context) : base(context)
    {
        _context = context;
    }

    public Task<int> FindNumberOfForksForRepository(Guid repositoryId)
    {
        return Task.FromResult(_context.RepositoryForks.Where(rf => rf.SourceRepoId.Equals(repositoryId)).ToList().Count);
    }
}