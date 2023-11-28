using Domain.Shared.Interfaces;

namespace Domain.Repositories.Interfaces;

public interface IRepositoryMemberRepository: IBaseRepository<RepositoryMember>
{
    Task<RepositoryMember?> FindByUserIdAndRepositoryId(Guid userId, Guid repositoryId);
    
    Task<RepositoryMember?> FindByRepositoryMemberIdAndRepositoryId(Guid repositoryMemberId, Guid repositoryId);
    Task<RepositoryMember> FindRepositoryOwner(Guid repositoryId);
    
    IEnumerable<RepositoryMember> FindRepositoryMembers(Guid repositoryId);

    int FindNumberRepositoryMembersThatAreOwnersExceptSingleMember(Guid repositoryId, Guid repositoryMemberId);
}