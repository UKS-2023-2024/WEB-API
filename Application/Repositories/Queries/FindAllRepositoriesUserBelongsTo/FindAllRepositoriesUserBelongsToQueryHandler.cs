using Application.Auth.Queries.FindById;
using Domain.Auth;
using Domain.Auth.Interfaces;
using Domain.Exceptions;
using Domain.Repositories;
using Domain.Repositories.Interfaces;
using MediatR;

namespace Application.Repositories.Queries.FindAllRepositoriesUserBelongsTo;

public class FindAllRepositoriesUserBelongsToQueryHandler : IRequestHandler<FindAllRepositoriesUserBelongsToQuery, IEnumerable<Repository>>
{
    private readonly IRepositoryRepository _repositoryRepository;
    public FindAllRepositoriesUserBelongsToQueryHandler(IRepositoryRepository repositoryRepository) => _repositoryRepository = repositoryRepository;

    public async Task<IEnumerable<Repository>> Handle(FindAllRepositoriesUserBelongsToQuery request, CancellationToken cancellationToken)
    {
        return await _repositoryRepository.FindAllUserBelongsTo(request.userId);
    }
}