using Application.Shared;
using Domain.Repositories;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;

namespace Application.Repositories.Commands.WatchingRepository.WatchRepository;

public class WatchRepositoryCommandHandler: ICommandHandler<WatchRepositoryCommand>
{
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly IRepositoryMemberRepository _repositoryMemberRepository;
    private readonly IRepositoryWatcherRepository _repositoryWatcherRepository;
    
    public WatchRepositoryCommandHandler(IRepositoryRepository repositoryRepository, IRepositoryMemberRepository repositoryMemberRepository, IRepositoryWatcherRepository repositoryWatcherRepository)
    {
        _repositoryRepository = repositoryRepository;
        _repositoryMemberRepository = repositoryMemberRepository;
        _repositoryWatcherRepository = repositoryWatcherRepository;
    }

    public async Task Handle(WatchRepositoryCommand request, CancellationToken cancellationToken)
    {
        var repository =  _repositoryRepository.Find(request.RepositoryId);
        Repository.ThrowIfDoesntExist(repository);

        var watcher = _repositoryWatcherRepository.FindByUserIdAndRepositoryId(request.User.Id, request.RepositoryId).Result;
        if (watcher != null)
        {
            watcher.ChangeWatchingPreferences(request.preferences);
            _repositoryWatcherRepository.Update(watcher);
            return;
        }

        var repositoryMember = await
            _repositoryMemberRepository.FindByUserIdAndRepositoryId(request.User.Id, request.RepositoryId);
        
        repository!.ThrowIfUserNotMemberAndRepositoryPrivate(repositoryMember);
        
        repository.AddToWatchedBy(request.User, request.preferences);
        _repositoryRepository.Update(repository);
    }
}