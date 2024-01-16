using Domain.Shared.Interfaces;

namespace Domain.Repositories.Interfaces;

public interface IRepositoryMemberRepository: IBaseRepository<RepositoryMember>
{
    Task<RepositoryMember?> FindByUserIdAndRepositoryId(Guid userId, Guid repositoryId);
    
    Task<RepositoryMember?> FindByRepositoryMemberIdAndRepositoryId(Guid repositoryMemberId, Guid repositoryId);
    Task<RepositoryMember> FindRepositoryOwner(Guid repositoryId);
    
    IEnumerable<RepositoryMember> FindRepositoryMembers(Guid repositoryId);
    Task<List<RepositoryMember>> FindAllByIds(Guid repositoryId, IEnumerable<Guid> memberIds);

    int FindNumberRepositoryMembersThatAreOwnersExceptSingleMember(Guid repositoryId, Guid repositoryMemberId);
}