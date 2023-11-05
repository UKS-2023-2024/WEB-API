using Domain.Shared.Interfaces;

namespace Domain.Repositories.Interfaces;

public interface IRepositoryRepository: IBaseRepository<Repository>
{
    Task<Repository> FindByNameAndOwner(string name, Guid ownerId);
    Task<Repository> FindByNameAndOrganization(string name, Guid organizationId);
}