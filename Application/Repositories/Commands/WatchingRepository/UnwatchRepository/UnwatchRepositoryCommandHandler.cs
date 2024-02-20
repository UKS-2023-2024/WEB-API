using Application.Shared;
using Domain.Repositories;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;

namespace Application.Repositories.Commands.WatchingRepository.UnwatchRepository;

public class UnwatchRepositoryCommandHandler: ICommandHandler<UnwatchRepositoryCommand>
{
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly IRepositoryWatcherRepository _repositoryWatcherRepository;
    
    public UnwatchRepositoryCommandHandler(IRepositoryRepository repositoryRepository, IRepositoryWatcherRepository repositoryWatcherRepository)
    {
        _repositoryRepository = repositoryRepository;
        _repositoryWatcherRepository = repositoryWatcherRepository;
    }

    public async Task Handle(UnwatchRepositoryCommand request, CancellationToken cancellationToken)
    {
        var repository =  _repositoryRepository.Find(request.RepositoryId);
        
        Repository.ThrowIfDoesntExist(repository);
        repository!.ThrowIfNotWatchedBy(request.User.Id);

        var watcher =_repositoryWatcherRepository.FindByUserIdAndRepositoryId(request.User.Id, repository.Id).Result;
        if (watcher == null)
            throw new RepositoryWatcherNotFoundException();
        repository.RemoveFromWatchedBy(watcher);
        _repositoryRepository.Update(repository);
    }
}