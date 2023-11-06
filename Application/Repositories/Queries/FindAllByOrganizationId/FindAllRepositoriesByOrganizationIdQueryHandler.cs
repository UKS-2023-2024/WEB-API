using Application.Auth.Queries.FindById;
using Domain.Auth;
using Domain.Auth.Interfaces;
using Domain.Exceptions;
using Domain.Repositories;
using Domain.Repositories.Interfaces;
using MediatR;

namespace Application.Repositories.Queries.FindAllByOrganizationId;

public class FindAllRepositoriesByOrganizationIdQueryHandler : IRequestHandler<FindAllRepositoriesByOrganizationIdQuery, IEnumerable<Repository>>
{
    private readonly IRepositoryRepository _repositoryRepository;
    public FindAllRepositoriesByOrganizationIdQueryHandler(IRepositoryRepository repositoryRepository) => _repositoryRepository = repositoryRepository;

    public async Task<IEnumerable<Repository>> Handle(FindAllRepositoriesByOrganizationIdQuery request, CancellationToken cancellationToken)
    {
        return await _repositoryRepository.FindAllByOrganizationId(request.organizationId);
    }
}