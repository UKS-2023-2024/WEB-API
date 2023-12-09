using Domain.Tasks;
using Domain.Tasks.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Shared.Repositories;

namespace Infrastructure.Tasks;

public class IssueRepository: BaseRepository<Issue>, IIssueRepository
{
    public IssueRepository(MainDbContext context) : base(context)
    {
    }
}