using Domain.Repositories;
using Domain.Shared;
using Domain.Shared.Interfaces;

namespace Domain.Branches.Interfaces;

public interface IBranchRepository : IBaseRepository<Branch>
{
    public Task<Branch?> FindByNameAndRepositoryId(string name, Guid repositoryId);
    public Task<Branch?> FindById(Guid branchId); 
    public Task<Branch?> FindByRepositoryIdAndIsDefault(Guid repositoryId, bool isDefault);
    public Task<IEnumerable<Branch>> FindAllByRepositoryIdAndIsDefault(Guid repositoryId, bool isDefault);
    public Task<PagedResult<Branch>> FindAllByRepositoryIdAndDeletedAndIsDefault(Guid repositoryId, bool deleted, bool isDefault, int pageSize, int PageNumber);
    public Task<PagedResult<Branch>> FindAllByRepositoryIdAndOwnerIdAndDeletedAndIsDefault(Guid repositoryId, Guid ownerId, bool deleted, bool isDefault, int pageSize, int PageNumber);
}