using Application.Shared;
using Domain.Organizations.Exceptions;
using Domain.Repositories;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;
using Domain.Shared.Interfaces;

namespace Application.Repositories.Commands.HandleRepositoryMembers.ChangeRole;

public class ChangeMemberRoleCommandHandler: ICommandHandler<ChangeMemberRoleCommand>
{
    private readonly IRepositoryMemberRepository _repositoryMemberRepository;
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly IGitService _gitService;

    public ChangeMemberRoleCommandHandler(IRepositoryMemberRepository repositoryMemberRepository, IRepositoryRepository repositoryRepository, IGitService gitService)
    {
        _repositoryMemberRepository = repositoryMemberRepository;
        _repositoryRepository = repositoryRepository;
        _gitService = gitService;
    }

    public async Task Handle(ChangeMemberRoleCommand request, CancellationToken cancellationToken)
    {
        var owner = await _repositoryMemberRepository.FindByUserIdAndRepositoryId(request.OwnerId, request.RepositoryId);
        RepositoryMember.ThrowIfDoesntExist(owner);
        owner!.ThrowIfNoAdminPrivileges();

        var member = await _repositoryMemberRepository
            .FindByRepositoryMemberIdAndRepositoryId(request.RepositoryMemberId,request.RepositoryId);
        
        RepositoryMember.ThrowIfDoesntExist(member);
        member!.ThrowIfSameAs(owner);
        
        if (request.Role == RepositoryMemberRole.OWNER || member.HasRole(RepositoryMemberRole.OWNER))
            throw new CantChangeOwnerException();

        member.SetRole(request.Role);
        _repositoryMemberRepository.Update(member);

        var repository = _repositoryRepository.Find(request.RepositoryId);
        Repository.ThrowIfDoesntExist(repository);
        
        await _gitService.RemoveRepositoryMember(repository!, member.Member);
        await _gitService.AddRepositoryMember(repository!, member.Member,request.Role.ToString("G").ToLower());
    }
}