using Domain.Milestones;
using Domain.Milestones.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Shared.Repositories;

namespace Infrastructure.Milestones;

public class MilestoneRepository : BaseRepository<Milestone>, IMilestoneRepository
{
    public MilestoneRepository(MainDbContext context) : base(context)
    {
    }
}