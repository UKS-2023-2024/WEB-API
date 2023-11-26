using Application.Repositories.Queries.FindAllNotDeletedByRepositoryId;
using Domain.Branches;
using Domain.Branches.Interfaces;
using MediatR;

namespace Application.Repositories.Queries.FindAllByOrganizationId;

public class FindDefaultBranchByRepositoryIdQueryHandler : IRequestHandler<FindDefaultBranchByRepositoryIdQuery, Branch>
{
    private readonly IBranchRepository _branchRepository;
    public FindDefaultBranchByRepositoryIdQueryHandler(IBranchRepository branchRepository) => _branchRepository = branchRepository;

    public async Task<Branch?> Handle(FindDefaultBranchByRepositoryIdQuery request, CancellationToken cancellationToken)
    {
        return await _branchRepository.FindByRepositoryIdAndIsDefault(request.repositoryId, true);
    }
}