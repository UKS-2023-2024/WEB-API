using Domain.Comments.Interfaces;
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
            .Include(i => i.Repository)
            .ThenInclude(r => r.WatchedBy)
            .Include(i => i.Milestone)
            .Include(i => i.Comments)
            .ThenInclude(c => c.Reactions)
            .Include(i => i.Events)
            .ThenInclude(x => x.Creator)
            .Include(i => i.Assignees)
            .Include(i => i.Labels)
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
            .Include(i => i.Labels)
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
    
    public Task<List<Issue>> FindAllByIds(Guid repositoryId, List<Guid> issueIds)
    {
        return _context.Issues
            .Where(
                issue => issueIds
                             .Any(issueId => 
                                 issueId.Equals(issue.Id)) && 
                         issue.RepositoryId.Equals(repositoryId)
            ).ToListAsync();
    }

    public async Task<List<Issue>> FindAllAssignedWithLabelInRepository(Label label, Guid repositoryId)
    {
        return await _context.Issues
            .Include(i => i.Labels)
            .Where(i => i.RepositoryId.Equals(repositoryId) && i.Labels.Contains(label))
            .ToListAsync();
    }
    
    public override async Task<Issue> Create(Issue issue)
    {
        issue.Created();
        await _context.Set<Issue>().AddAsync(issue);
        await _context.SaveChangesAsync();
        return issue;
    }

    public override void Update(Issue issue)
    {
        issue.Updated();
        _context.Update(issue);
        _context.SaveChanges();
    }
}