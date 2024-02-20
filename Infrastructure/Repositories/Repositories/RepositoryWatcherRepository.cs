using Domain.Repositories;
using Domain.Repositories.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Shared.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Repositories;

public class RepositoryWatcherRepository: BaseRepository<RepositoryWatcher>, IRepositoryWatcherRepository
{

    private readonly MainDbContext _context;
    public RepositoryWatcherRepository(MainDbContext context) : base(context)
    {
        _context = context;
    }
    
    public Task<RepositoryWatcher?> FindByUserIdAndRepositoryId(Guid userId, Guid repositoryId)
    {
        return _context.RepositoryWatchers
            .Where(r => r.User.Id.Equals(userId) && r.RepositoryId.Equals(repositoryId))
            .FirstOrDefaultAsync();
    }
}