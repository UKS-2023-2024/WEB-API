using Domain.Branches;
using Domain.Branches.Interfaces;
using Domain.Shared;
using MediatR;

namespace Application.Repositories.Queries.FindAllUserActiveByRepositoryId;

public class FindAllUserActiveBranchesByRepositoryIdQueryHandler : IRequestHandler<FindAllUserActiveBranchesByRepositoryIdQuery, PagedResult<Branch>>
{
    private readonly IBranchRepository _branchRepository;
    public FindAllUserActiveBranchesByRepositoryIdQueryHandler(IBranchRepository branchRepository) => _branchRepository = branchRepository;

    public async Task<PagedResult<Branch>> Handle(FindAllUserActiveBranchesByRepositoryIdQuery request, CancellationToken cancellationToken)
    {
        return await _branchRepository.FindAllByRepositoryIdAndOwnerIdAndDeletedAndIsDefault(request.RepositoryId, request.OwnerId, false, false, request.PageSize, request.PageNumber);
    }
}