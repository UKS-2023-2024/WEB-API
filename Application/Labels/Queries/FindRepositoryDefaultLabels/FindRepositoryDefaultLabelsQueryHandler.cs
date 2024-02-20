using Application.Shared;
using Domain.Repositories.Interfaces;
using Domain.Tasks;

namespace Application.Labels.Commands.Queries.FindRepositoryDefaultLabels;

public class FindRepositoryDefaultLabelsQueryHandler: IQueryHandler<FindRepositoryDefaultLabelsQuery, List<Label>>
{
    private readonly IRepositoryRepository _repositoryRepository;

    public FindRepositoryDefaultLabelsQueryHandler(IRepositoryRepository repositoryRepository)
    {
        _repositoryRepository = repositoryRepository;
    }

    public async Task<List<Label>> Handle(FindRepositoryDefaultLabelsQuery request, CancellationToken cancellationToken)
    {
        return await _repositoryRepository.FindRepositoryLabels(request.RepositoryId, request.Search);
    }
}