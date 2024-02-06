using Application.Shared;
using Domain.Auth;
using Domain.Auth.Interfaces;
using Domain.Repositories;
using Domain.Repositories.Interfaces;
using Domain.Shared.Interfaces;

namespace Application.Repositories.Commands.HandleRepositoryMembers.AddRepositoryMember;

public class AddRepositoryMemberCommandHandler : ICommandHandler<AddRepositoryMemberCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IRepositoryInviteRepository _repositoryInviteRepository;
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly IGitService _gitService;

    public AddRepositoryMemberCommandHandler(
        IUserRepository userRepository, 
        IRepositoryInviteRepository repositoryInviteRepository, 
        IRepositoryRepository repositoryRepository,
        IGitService gitService)
    {
        _userRepository = userRepository;
        _repositoryInviteRepository = repositoryInviteRepository;
        _repositoryRepository = repositoryRepository;
        _gitService = gitService;
    }
    
    
    public async Task Handle(AddRepositoryMemberCommand request, CancellationToken cancellationToken)
    {
        var invite = _repositoryInviteRepository.Find(request.InviteId);
        RepositoryInvite.ThrowIfDoesntExist(invite);
        invite!.ThrowIfExpired();

        var repository = _repositoryRepository.Find(invite.RepositoryId);
        Repository.ThrowIfDoesntExist(repository);
        
        var user = await _userRepository.FindUserById(invite.UserId);
        User.ThrowIfDoesntExist(user);

        repository!.AddMember(user!);
        _repositoryRepository.Update(repository);
        _repositoryInviteRepository.Delete(invite);
        
        await _gitService.AddRepositoryMember(repository, user!,"read");
    }
}