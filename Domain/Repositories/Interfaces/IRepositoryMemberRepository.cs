using Domain.Shared.Interfaces;

namespace Domain.Repositories.Interfaces;

public interface IRepositoryMemberRepository: IBaseRepository<RepositoryMember>
{
    Task<RepositoryMember> FindByUserIdAndRepositoryId(Guid userId, Guid repositoryId);
    Task<RepositoryMember> FindRepositoryOwner(Guid repositoryId);
}