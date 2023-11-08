using Application.Repositories.Commands.Delete;
using Application.Shared;
using Domain.Repositories;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;

namespace Application.Repositories.Commands.StarRepository;

public class StarRepositoryCommandHandler: ICommandHandler<StarRepositoryCommand>
{
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly IRepositoryMemberRepository _repositoryMemberRepository;
    
    public StarRepositoryCommandHandler(IRepositoryRepository repositoryRepository, IRepositoryMemberRepository repositoryMemberRepository)
    {
        _repositoryRepository = repositoryRepository;
        _repositoryMemberRepository = repositoryMemberRepository;
    }

    public async Task Handle(StarRepositoryCommand request, CancellationToken cancellationToken)
    {
        var repository =  _repositoryRepository.Find(request.repositoryId);
        
        if (repository is null)
            throw new RepositoryNotFoundException();
        var repositoryMember = await
            _repositoryMemberRepository.FindByUserIdAndRepositoryId(request.user.Id, request.repositoryId);
        if(repository.IsPrivate && repositoryMember == null)
            throw new RepositoryInaccessibleException();
        if(repository.StarredBy.Any(user=> user.Id == request.user.Id))
            throw new RepositoryAlreadyStarredException();
        
        repository.AddToStarredBy(request.user);
        _repositoryRepository.Update(repository);
    }
}