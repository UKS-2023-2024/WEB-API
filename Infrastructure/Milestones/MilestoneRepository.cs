using Domain.Milestones;
using Domain.Milestones.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Shared.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Milestones;

public class MilestoneRepository : BaseRepository<Milestone>, IMilestoneRepository
{
    private readonly MainDbContext _context;
    public MilestoneRepository(MainDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<Milestone>> FindActiveRepositoryMilestones(Guid repositoryId)
    {
        return _context.Milestones
            .Where(m => m.RepositoryId.Equals(repositoryId) && !m.Closed)
            .ToList();
    }

    public async Task<List<Milestone>> FindClosedRepositoryMilestones(Guid repositoryId)
    {
        return _context.Milestones
            .Where(m => m.RepositoryId.Equals(repositoryId) && m.Closed)
            .ToList();
    }

    public async Task<Milestone> FindMilestone(Guid milestoneId)
    {
        return await _context.Milestones
            .Where(m => m.Id.Equals(milestoneId))
            .Include(m => m.Tasks)
            .ThenInclude(t => t.Labels)
            .Include(m => m.Tasks)
            .ThenInclude(t => t.Creator)
            .Include(m => m.Tasks)
            .ThenInclude(t => t.Events)
            .FirstAsync();
    }
}