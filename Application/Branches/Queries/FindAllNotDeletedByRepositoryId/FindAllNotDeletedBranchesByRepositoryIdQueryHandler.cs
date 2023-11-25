using Application.Auth.Queries.FindById;
using Application.Repositories.Queries.FindAllNotDeletedByRepositoryId;
using Domain.Auth;
using Domain.Auth.Interfaces;
using Domain.Branches;
using Domain.Branches.Interfaces;
using Domain.Exceptions;
using Domain.Repositories;
using Domain.Repositories.Interfaces;
using MediatR;

namespace Application.Repositories.Queries.FindAllByOrganizationId;

public class FindAllNotDeletedBranchesByRepositoryIdQueryHandler : IRequestHandler<FindAllNotDeletedBranchesByRepositoryIdQuery, IEnumerable<Branch>>
{
    private readonly IBranchRepository _branchRepository;
    public FindAllNotDeletedBranchesByRepositoryIdQueryHandler(IBranchRepository branchRepository) => _branchRepository = branchRepository;

    public async Task<IEnumerable<Branch>> Handle(FindAllNotDeletedBranchesByRepositoryIdQuery request, CancellationToken cancellationToken)
    {
        return await _branchRepository.FindAllNotDeletedByRepositoryId(request.repositoryId);
    }
}