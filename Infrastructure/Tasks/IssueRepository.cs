using Domain.Tasks;
using Domain.Tasks.Enums;
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
        return _context.Issues.Where(i => i.Id.Equals(id))
            .Include(i => i.Milestone)
            .Include(i => i.Events)
            .ThenInclude(x => x.Creator)
            .Include(i => i.Assignees)
            .ToList()
            .Select(x =>
                {
                    x.Events = x.Events.OrderBy(e => e.CreatedAt).ToList();
                    return x;
                }
            )
            .First();
    }

    public async Task<List<Issue>> FindRepositoryIssues(Guid repositoryId)
    {
        return _context.Issues
            .Where(i => i.RepositoryId.Equals(repositoryId))
            .Include(x => x.Events)
            .ThenInclude(x => x.Creator)
            .ToList()
            .Select(x =>
                {
                    x.Events = x.Events.OrderBy(e => e.CreatedAt).ToList();
                    return x;
                }
            )
            .ToList();
    }

    public async Task<List<Event>> FindOpenedIssueEvents(Guid issueId)
    {
        return await _context.Events.Where(e => e.TaskId.Equals(issueId))
            .OrderByDescending(e => e.CreatedAt)
            .ToListAsync();
    }
    
    public async Task<List<Issue>> FindMilestoneIssues(Guid milestoneId)
    {
        return await _context.Issues.Where(e => e.MilestoneId.Equals(milestoneId))
               .ToListAsync();
    }
}