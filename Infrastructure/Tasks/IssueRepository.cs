using Domain.Tasks;
using Domain.Tasks.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Shared.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tasks;

public class IssueRepository: BaseRepository<Issue>, IIssueRepository
{
    private readonly MainDbContext _context;
    public IssueRepository(MainDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Issue> FindById(Guid id)
    {
        return await _context.Issues.Where(i => i.Id.Equals(id))
            .Include(i => i.Milestone)
            .Include(i => i.Events)
            .Include(i => i.Assignees)
            .FirstAsync();
    }

    public async Task<List<Issue>> FindRepositoryIssues(Guid repositoryId)
    {
        return await _context.Issues
            .Where(i => i.RepositoryId.Equals(repositoryId))
            .ToListAsync();
    }
}