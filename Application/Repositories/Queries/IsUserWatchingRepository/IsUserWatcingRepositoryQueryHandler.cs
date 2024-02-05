using Domain.Repositories;
using Domain.Repositories.Enums;
using Domain.Repositories.Interfaces;
using MediatR;

namespace Application.Repositories.Queries.IsUserWatchingRepository;

public class IsUserWatchingRepositoryQueryHandler : IRequestHandler<IsUserWatchingRepositoryQuery, WatchingPreferences?>
{
    private readonly IRepositoryWatcherRepository _repositoryWatcherRepository;
    public IsUserWatchingRepositoryQueryHandler(IRepositoryWatcherRepository repositoryWatcherRepository)
    {
        _repositoryWatcherRepository = repositoryWatcherRepository;
    }
    
    public async Task<WatchingPreferences?> Handle(IsUserWatchingRepositoryQuery request, CancellationToken cancellationToken)
    {
       RepositoryWatcher watcher = await _repositoryWatcherRepository.FindByUserIdAndRepositoryId(request.UserId, request.RepositoryId);
        if (watcher == null)
        {
            return null;
        }
        return watcher.WatchingPreferences;
    }
}