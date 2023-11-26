using Domain.Branches;
using Domain.Branches.Interfaces;
using Domain.Shared;
using MediatR;

namespace Application.Repositories.Queries.FindAllActiveByRepositoryId;

public class FindAllActiveBranchesByRepositoryIdQueryHandler : IRequestHandler<FindAllActiveBranchesByRepositoryIdQuery, PagedResult<Branch>>
{
    private readonly IBranchRepository _branchRepository;
    public FindAllActiveBranchesByRepositoryIdQueryHandler(IBranchRepository branchRepository) => _branchRepository = branchRepository;

    public async Task<PagedResult<Branch>> Handle(FindAllActiveBranchesByRepositoryIdQuery request, CancellationToken cancellationToken)
    {
        return await _branchRepository.FindAllByRepositoryIdAndDeletedAndIsDefault(request.RepositoryId, false, false, request.PageSize, request.PageNumber);
    }
}