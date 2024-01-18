using Domain.Tasks;
using Domain.Tasks.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Shared.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tasks;

public class LabelRepository: BaseRepository<Label>, ILabelRepository
{
    private readonly MainDbContext _context;
    public LabelRepository(MainDbContext context) : base(context)
    {
        _context = context;
    }

    public Task<List<Label>> FindAllByIds(Guid repositoryId, List<Guid> labelsIds)
    {
        return _context.Labels
            .Where(
                label => labelsIds
                             .Any(labelId => 
                                 labelId.Equals(label.Id)) && 
                         label.RepositoryId.Equals(repositoryId)
            ).ToListAsync();
    }
}