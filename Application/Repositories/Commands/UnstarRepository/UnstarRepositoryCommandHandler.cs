using Application.Shared;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;

namespace Application.Repositories.Commands.UnstarRepository;

public class UnstarRepositoryCommandHandler: ICommandHandler<UnstarRepositoryCommand>
{
    private readonly IRepositoryRepository _repositoryRepository;
    
    public UnstarRepositoryCommandHandler(IRepositoryRepository repositoryRepository)
    {
        _repositoryRepository = repositoryRepository;
    }

    public async Task Handle(UnstarRepositoryCommand request, CancellationToken cancellationToken)
    {
        var repository =  _repositoryRepository.Find(request.repositoryId);
        
        if (repository is null)
            throw new RepositoryNotFoundException();
        if(repository.StarredBy.All(u => u.Id != request.user.Id))
            throw new RepositoryNotStarredException();

        repository.RemoveFromStarredBy(request.user);
        _repositoryRepository.Update(repository);
    }
}