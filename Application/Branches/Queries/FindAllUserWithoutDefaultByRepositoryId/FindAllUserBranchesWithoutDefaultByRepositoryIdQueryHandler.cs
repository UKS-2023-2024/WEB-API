using Domain.Branches;
using Domain.Branches.Interfaces;
using Domain.Shared;
using MediatR;

namespace Application.Repositories.Queries.FindAllUserWithoutDefaultByRepositoryId;

public class FindAllUserBranchesWithoutDefaultByRepositoryIdQueryHandler : IRequestHandler<FindAllUserBranchesWithoutDefaultByRepositoryIdQuery, PagedResult<Branch>>
{
    private readonly IBranchRepository _branchRepository;
    public FindAllUserBranchesWithoutDefaultByRepositoryIdQueryHandler(IBranchRepository branchRepository) => _branchRepository = branchRepository;

    public async Task<PagedResult<Branch>> Handle(FindAllUserBranchesWithoutDefaultByRepositoryIdQuery request, CancellationToken cancellationToken)
    {
        return await _branchRepository.FindAllByRepositoryIdAndOwnerIdAndDeletedAndIsDefault(request.RepositoryId, request.OwnerId, false, false, request.PageSize, request.PageNumber);
    }
}