﻿using Application.Shared;
using Domain.Repositories;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;

namespace Application.Repositories.Commands.HandleRepositoryMembers.ChangeRole;

public class ChangeMemberRoleCommandHandler: ICommandHandler<ChangeMemberRoleCommand>
{
    private readonly IRepositoryMemberRepository _repositoryMemberRepository;
    private readonly IRepositoryRepository _repositoryRepository;

    public ChangeMemberRoleCommandHandler(IRepositoryMemberRepository repositoryMemberRepository, IRepositoryRepository repositoryRepository)
    {
        _repositoryMemberRepository = repositoryMemberRepository;
        _repositoryRepository = repositoryRepository;
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
        
        var numberOfOwners =
            _repositoryMemberRepository.FindNumberRepositoryMembersThatAreOwnersExceptSingleMember(request.RepositoryId,
                request.RepositoryMemberId);
        if (numberOfOwners <= 0)
            throw new RepositoryMemberCantBeChangedException();

        member.SetRole(request.Role);
        _repositoryMemberRepository.Update(member);
    }
}