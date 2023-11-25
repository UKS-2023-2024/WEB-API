using Domain.Shared.Interfaces;

namespace Domain.Branches.Interfaces;

public interface IBranchRepository : IBaseRepository<Branch>
{
    public Task<Branch?> FindByNameAndRepositoryId(string name, Guid repositoryId);
}