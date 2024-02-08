using Application.Shared;
using Domain.Branches;
using Domain.Branches.Interfaces;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;

namespace Application.Branches.Queries.FindAllRepositoryBranches;

public class FindAllRepositoryBranchesQueryHandler : IQueryHandler<FindAllRepositoryBranchesQuery,IEnumerable<Branch>>
{
    private readonly IRepositoryMemberRepository _repositoryMemberRepository;
    private readonly IBranchRepository _branchRepository;

    public FindAllRepositoryBranchesQueryHandler(IRepositoryMemberRepository repositoryMemberRepository,
        IBranchRepository branchRepository)
    {
        _repositoryMemberRepository = repositoryMemberRepository;
        _branchRepository = branchRepository;
    }

    public async Task<IEnumerable<Branch>> Handle(FindAllRepositoryBranchesQuery request, CancellationToken cancellationToken)
    {
        var repoMember = await _repositoryMemberRepository.FindByUserIdAndRepositoryId(request.UserId, request.RepositoryId);
        if (repoMember is null)
            throw new RepositoryInaccessibleException();
        return await _branchRepository.FindAllRepositoryBranches(request.RepositoryId);
    }
}