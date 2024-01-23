using Application.Shared;
using Domain.Repositories;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;

namespace Application.Repositories.Commands.WatchingRepository.UnwatchRepository;

public class UnwatchRepositoryCommandHandler: ICommandHandler<UnwatchRepositoryCommand>
{
    private readonly IRepositoryRepository _repositoryRepository;
    
    public UnwatchRepositoryCommandHandler(IRepositoryRepository repositoryRepository)
    {
        _repositoryRepository = repositoryRepository;
    }

    public async Task Handle(UnwatchRepositoryCommand request, CancellationToken cancellationToken)
    {
        var repository =  _repositoryRepository.Find(request.RepositoryId);
        
        Repository.ThrowIfDoesntExist(repository);
        repository!.ThrowIfNotWatchedBy(request.User.Id);

        repository.RemoveFromWatchedBy(request.User);
        _repositoryRepository.Update(repository);
    }
}