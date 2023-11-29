using Domain.Repositories.Interfaces;
using MediatR;

namespace Application.Repositories.Queries.DidUserStarRepository;

public class DidUserStarRepositoryQueryHandler: IRequestHandler<DidUserStarRepositoryQuery, bool>
{
    private readonly IRepositoryRepository _repositoryRepository;
    public DidUserStarRepositoryQueryHandler(IRepositoryRepository repositoryRepository)
    {
        _repositoryRepository = repositoryRepository;
    }
    
    public async Task<bool> Handle(DidUserStarRepositoryQuery request, CancellationToken cancellationToken)
    {
       return await _repositoryRepository.DidUserStarRepository(request.UserId,request.RepositoryId);
    }
}