using Domain.Repositories;
using Domain.Repositories.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Shared.Repositories;

namespace Infrastructure.Repositories.Repositories;

public class RepositoryForkRepository: BaseRepository<RepositoryFork>, IRepositoryForkRepository
{
    public RepositoryForkRepository(MainDbContext context) : base(context)
    {
    }
}