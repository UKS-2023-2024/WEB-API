using Domain.Branches;
using Domain.Branches.Interfaces;
using Domain.Shared;
using MediatR;

namespace Application.Repositories.Queries.FindAllWithoutDefaultByRepositoryIdPagination;

public class FindAllBranchesWithoutDefaultByRepositoryIdPaginationQueryHandler : IRequestHandler<FindAllBranchesWithoutDefaultByRepositoryIdPaginationQuery, PagedResult<Branch>>
{
    private readonly IBranchRepository _branchRepository;
    public FindAllBranchesWithoutDefaultByRepositoryIdPaginationQueryHandler(IBranchRepository branchRepository) => _branchRepository = branchRepository;

    public async Task<PagedResult<Branch>> Handle(FindAllBranchesWithoutDefaultByRepositoryIdPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _branchRepository.FindAllByRepositoryIdAndDeletedAndIsDefault(request.RepositoryId, false, false, request.PageSize, request.PageNumber);
    }
}