using Application.Shared;
using Domain.Repositories;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;

namespace Application.Repositories.Commands.StarringRepository.UnstarRepository;

public class UnstarRepositoryCommandHandler: ICommandHandler<UnstarRepositoryCommand>
{
    private readonly IRepositoryRepository _repositoryRepository;
    
    public UnstarRepositoryCommandHandler(IRepositoryRepository repositoryRepository)
    {
        _repositoryRepository = repositoryRepository;
    }

    public async Task Handle(UnstarRepositoryCommand request, CancellationToken cancellationToken)
    {
        var repository =  _repositoryRepository.Find(request.RepositoryId);
        
        Repository.ThrowIfDoesntExist(repository);
        repository!.ThrowIfNotStarredBy(request.User.Id);

        repository.RemoveFromStarredBy(request.User);
        _repositoryRepository.Update(repository);
    }
}