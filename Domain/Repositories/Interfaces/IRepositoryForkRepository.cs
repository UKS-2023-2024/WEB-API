using Domain.Shared.Interfaces;

namespace Domain.Repositories.Interfaces;

public interface IRepositoryForkRepository: IBaseRepository<RepositoryFork>
{
    Task<int> FindNumberOfForksForRepository(Guid repositoryId);
}