using Domain.Shared.Interfaces;

namespace Domain.Repositories.Interfaces;

public interface IRepositoryInviteRepository: IBaseRepository<RepositoryInvite>
{
    RepositoryInvite? FindByRepoAndMember(Guid repositoryId, Guid memberId);
    Task<RepositoryInvite?> FindById(Guid id);
}