using Application.Shared;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;

namespace Application.Repositories.Commands.StarringRepository.StarRepository;

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
        var repository =  _repositoryRepository.Find(request.RepositoryId);
        
        if (repository is null)
            throw new RepositoryNotFoundException();
        var repositoryMember = await
            _repositoryMemberRepository.FindByUserIdAndRepositoryId(request.User.Id, request.RepositoryId);
        if(repository.IsPrivate && repositoryMember == null)
            throw new RepositoryInaccessibleException();
        if(repository.StarredBy.Any(user=> user.Id == request.User.Id))
            throw new RepositoryAlreadyStarredException();
        
        repository.AddToStarredBy(request.User);
        _repositoryRepository.Update(repository);
    }
}