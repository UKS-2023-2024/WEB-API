﻿using Application.Shared;
using Domain.Repositories;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;
using Domain.Shared.Interfaces;

namespace Application.Repositories.Commands.HandleRepositoryMembers.RemoveRepositoryMember;

public class RemoveRepositoryMemberCommandHandler: ICommandHandler<RemoveRepositoryMemberCommand>
{
    
    private readonly IRepositoryMemberRepository _repositoryMemberRepository;
    private readonly IRepositoryRepository _repositoryRepository;
    private readonly IGitService _gitService;

    public RemoveRepositoryMemberCommandHandler(
        IRepositoryRepository repositoryRepository,
        IRepositoryMemberRepository repositoryMemberRepository,
        IGitService gitService)
    {
        _repositoryRepository = repositoryRepository;
        _repositoryMemberRepository = repositoryMemberRepository;
        _gitService = gitService;
    }

    public async Task Handle(RemoveRepositoryMemberCommand request, CancellationToken cancellationToken)
    {
        var owner = await _repositoryMemberRepository.FindByUserIdAndRepositoryId(request.OwnerId, request.RepositoryId);
        RepositoryMember.ThrowIfDoesntExist(owner);
        owner!.ThrowIfNoAdminPrivileges();

        var member = _repositoryMemberRepository.Find(request.RepositoryMemberId);
        RepositoryMember.ThrowIfDoesntExist(member);
        member!.ThrowIfSameAs(owner);
        
        var numberOfOwners =
            _repositoryMemberRepository.FindNumberRepositoryMembersThatAreOwnersExceptSingleMember(request.RepositoryId,
                request.RepositoryMemberId);
        if (numberOfOwners <= 0)
            throw new RepositoryMemberCantBeDeletedException();
        var repository = _repositoryRepository.Find(request.RepositoryId);

        repository!.RemoveMember(member!);
        _repositoryRepository.Update(repository);
        await _gitService.RemoveRepositoryMember(repository, member.Member);
    }
}