using Application.Shared;
using Domain.Repositories;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;

namespace Application.Repositories.Commands.WatchingRepository.WatchRepository;

public class WatchRepositoryCommandHandler: ICommandHandler<WatchRepositoryCommand>
{
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly IRepositoryMemberRepository _repositoryMemberRepository;
    
    public WatchRepositoryCommandHandler(IRepositoryRepository repositoryRepository, IRepositoryMemberRepository repositoryMemberRepository)
    {
        _repositoryRepository = repositoryRepository;
        _repositoryMemberRepository = repositoryMemberRepository;
    }

    public async Task Handle(WatchRepositoryCommand request, CancellationToken cancellationToken)
    {
        var repository =  _repositoryRepository.Find(request.RepositoryId);
        
        Repository.ThrowIfDoesntExist(repository);
        
        var repositoryMember = await
            _repositoryMemberRepository.FindByUserIdAndRepositoryId(request.User.Id, request.RepositoryId);
        
        repository!.ThrowIfUserNotMemberAndRepositoryPrivate(repositoryMember);
        repository.ThrowIfAlreadyWatchedBy(request.User.Id);
        
        repository.AddToWatchedBy(request.User);
        _repositoryRepository.Update(repository);
    }
}