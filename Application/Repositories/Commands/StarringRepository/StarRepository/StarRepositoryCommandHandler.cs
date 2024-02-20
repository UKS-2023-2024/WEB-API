using Application.Shared;
using Domain.Repositories;
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
        
        Repository.ThrowIfDoesntExist(repository);
        
        var repositoryMember = await
            _repositoryMemberRepository.FindByUserIdAndRepositoryId(request.User.Id, request.RepositoryId);
        
        repository!.ThrowIfUserNotMemberAndRepositoryPrivate(repositoryMember);
        repository.ThrowIfAlreadyStarredBy(request.User.Id);
        
        repository.AddToStarredBy(request.User);
        _repositoryRepository.Update(repository);
    }
}