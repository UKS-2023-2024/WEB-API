using Domain.Repositories.Enums;
using Domain.Shared.Interfaces;

namespace Domain.Repositories.Interfaces;

public interface IRepositoryWatcherRepository: IBaseRepository<RepositoryWatcher>
{
    Task<RepositoryWatcher?> FindByUserIdAndRepositoryId(Guid userId, Guid repositoryId);
}