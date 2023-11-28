using Application.Shared;
using Domain.Repositories;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;

namespace Application.Repositories.Commands.HandleRepositoryMembers.RemoveRepositoryMember;

public class RemoveRepositoryMemberCommandHandler: ICommandHandler<RemoveRepositoryMemberCommand>
{
    
    private readonly IRepositoryMemberRepository _repositoryMemberRepository;
    private readonly IRepositoryRepository _repositoryRepository;

    public RemoveRepositoryMemberCommandHandler(IRepositoryRepository repositoryRepository, IRepositoryMemberRepository repositoryMemberRepository)
    {
        _repositoryRepository = repositoryRepository;
        _repositoryMemberRepository = repositoryMemberRepository;
    }

    public async Task Handle(RemoveRepositoryMemberCommand request, CancellationToken cancellationToken)
    {
        var owner = await _repositoryMemberRepository.FindByUserIdAndRepositoryId(request.OwnerId, request.RepositoryId);
        RepositoryMember.ThrowIfDoesntExist(owner);
        owner!.ThrowIfNoAdminPrivileges();

        var member = _repositoryMemberRepository.Find(request.RepositoryMemberId);
        if (member is null || member.Deleted) throw new RepositoryMemberNotFoundException();

        if (_repositoryMemberRepository.FindNumberRepositoryMembersThatAreOwnersExceptSingleMember(request.RepositoryId, request.RepositoryMemberId) <= 0)
            throw new RepositoryMemberCantBeDeletedException();
        var repository = _repositoryRepository.Find(request.RepositoryId);

        repository!.RemoveMember(member);
        _repositoryRepository.Update(repository);
    }
}