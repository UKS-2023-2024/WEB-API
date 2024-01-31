using Domain.Auth;
using Domain.Shared.Interfaces;

namespace Domain.Repositories.Interfaces;

public interface IRepositoryRepository: IBaseRepository<Repository>
{
    Task<Repository> FindByNameAndOwnerId(string name, Guid ownerId);
    Task<Repository> FindByNameAndOrganizationId(string name, Guid organizationId);
    Task<IEnumerable<Repository>> FindAllByOwnerId(Guid id);
    Task<IEnumerable<Repository>> FindAllByOrganizationId(Guid id);
    Task<IEnumerable<Repository>> FindOrganizationRepositoriesThatContainsUser(Guid userId,Guid organizationId);
    
    Task<bool> DidUserStarRepository(Guid userid,Guid repositoryId);
    Task<IEnumerable<Repository>> FindAllUserBelongsTo(Guid id);
    Task<bool> IsUserWatchingRepository(Guid userid, Guid repositoryId);
    Task<User?> FindRepositoryOwner(Guid repositoryId);
}