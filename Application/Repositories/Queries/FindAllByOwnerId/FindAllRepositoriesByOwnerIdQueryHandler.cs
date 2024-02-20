using Application.Auth.Queries.FindById;
using Domain.Auth;
using Domain.Auth.Interfaces;
using Domain.Exceptions;
using Domain.Repositories;
using Domain.Repositories.Interfaces;
using MediatR;

namespace Application.Repositories.Queries.FindAllByOwnerId;

public class FindAllRepositoriesByOwnerIdQueryHandler : IRequestHandler<FindAllRepositoriesByOwnerIdQuery, IEnumerable<Repository>>
{
    private readonly IRepositoryRepository _repositoryRepository;
    public FindAllRepositoriesByOwnerIdQueryHandler(IRepositoryRepository repositoryRepository) => _repositoryRepository = repositoryRepository;

    public async Task<IEnumerable<Repository>> Handle(FindAllRepositoriesByOwnerIdQuery request, CancellationToken cancellationToken)
    {
        return await _repositoryRepository.FindAllByOwnerId(request.ownerId);
    }
}