using Domain.Repositories.Interfaces;
using MediatR;

namespace Application.Repositories.Queries.IsUserWatchingRepository;

public class IsUserWatchingRepositoryQueryHandler: IRequestHandler<IsUserWatchingRepositoryQuery, bool>
{
    private readonly IRepositoryRepository _repositoryRepository;
    public IsUserWatchingRepositoryQueryHandler(IRepositoryRepository repositoryRepository)
    {
        _repositoryRepository = repositoryRepository;
    }
    
    public async Task<bool> Handle(IsUserWatchingRepositoryQuery request, CancellationToken cancellationToken)
    {
       return await _repositoryRepository.IsUserWatchingRepository(request.UserId,request.RepositoryId);
    }
}