using Domain.Branches;
using Domain.Branches.Interfaces;
using MediatR;

namespace Application.Repositories.Queries.FindAllWithoutDefaultByRepositoryId;

public class FindAllBranchesWithoutDefaultByRepositoryIdQueryHandler : IRequestHandler<FindAllBranchesWithoutDefaultByRepositoryIdQuery, IEnumerable<Branch>>
{
    private readonly IBranchRepository _branchRepository;
    public FindAllBranchesWithoutDefaultByRepositoryIdQueryHandler(IBranchRepository branchRepository) => _branchRepository = branchRepository;

    public async Task<IEnumerable<Branch>> Handle(FindAllBranchesWithoutDefaultByRepositoryIdQuery request, CancellationToken cancellationToken)
    {
        return await _branchRepository.FindAllByRepositoryIdAndIsDefault(request.repositoryId, false);
    }
}